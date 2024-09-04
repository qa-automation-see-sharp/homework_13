using System.Net;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api.Tests
{
    public class CreateBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private readonly LibraryHttpService _libraryHttpService;
        private Book _book;

        public CreateBookTests(LibraryHttpService libraryHttpService)
        {
            _libraryHttpService = libraryHttpService;
        }

        public async Task InitializeAsync()
        {
            ConsoleHelper.SetUpFromClass(GetType().Name);
        }

        [Theory]
        [InlineData("Philosopher's Stone", "Joanne Rowling", 1997)]
        [InlineData("Chamber of Secrets", "Joanne Rowling", 1998)]
        [InlineData("Prisoner of Azkaban", "Joanne Rowling", 1999)]
        [InlineData("Goblet of Fire ", "Joanne Rowling", 2000)]
        [InlineData("Order of the Phoenix", "Joanne Rowling", 2003)]
        [InlineData("Half-Blood Prince", "Joanne Rowling", 2005)]
        public async Task CreateBook_ReturnCreated(string title, string author, int year)
        {
            _book = DataHelper.BookHelper.BookWithTitleAuthorYear(title, author, year);

            var obj = await _libraryHttpService.PostBook(_book);
            var response = await obj.Content.ReadAsStringAsync();
            var bookObj = JsonConvert.DeserializeObject<Book>(response);

            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, obj.StatusCode);
                Assert.Equal(_book.Title, bookObj.Title);
                Assert.Equal(_book.Author, bookObj.Author);
                Assert.Equal(_book.YearOfRelease, bookObj.YearOfRelease);
            });
        }

        [Fact]
        public new async Task CreateExistedBook_ReturnBadRequest()
        {
            _book = DataHelper.BookHelper.RandomBook();

            await _libraryHttpService.PostBook(_book);
            var obj = await _libraryHttpService.PostBook(_book);
            var response = await obj.Content.ReadAsStringAsync();
            var s = response.Trim('"');

            Assert.Multiple(() =>
           {
               Assert.Equal(obj.StatusCode, HttpStatusCode.BadRequest);
               Assert.NotNull(response);
               Assert.Contains(DataHelper.ErrorMessage.ExistBook(_book), s);
           });
        }
        public async Task DisposeAsync()
        {
            ConsoleHelper.TearDownFromClass(GetType().Name);
            await _libraryHttpService.DeleteBook(_book.Title, _book.Author);
        }
    }
}