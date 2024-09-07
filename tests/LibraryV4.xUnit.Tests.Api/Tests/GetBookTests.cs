using System.Net;
using LibraryV4.Common.Files.For.Tests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api.Tests
{
    public class GetBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private readonly LibraryHttpService _libraryHttpService;
        private Book _book;

        public GetBookTests(LibraryHttpService libraryHttpService)
        {
            _libraryHttpService = libraryHttpService;
        }

        public async Task InitializeAsync()
        {
            ConsoleHelper.SetUpFromClass(GetType().Name);
            _book = DataHelper.BookHelper.RandomBook();
            await _libraryHttpService.PostBook(_book);
        }

        [Fact]
        public async Task GetBookByTitle_Return_OK()
        {
            var book = await _libraryHttpService.GetBooksByTitle(_book.Title);
            var booksJsonString = await book.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(booksJsonString);

            Assert.Multiple(() =>
                   {
                       Assert.NotNull(books);
                       Assert.NotEmpty(books);
                       Assert.Single(books);
                       Assert.Equal(books[0].Title, _book.Title);
                       Assert.Equal(books[0].Author, _book.Author);
                       Assert.Equal(books[0].YearOfRelease, _book.YearOfRelease);
                   });

        }

        [Fact]
        public async Task GetBookByTitle_Return_NotFound()
        {
            var book = DataHelper.BookHelper.BookWithTitleAuthorYear("Not Found", "Not Found Author", 1990);
            var response = await _libraryHttpService.GetBooksByTitle(book.Title);
            var message = await response.Content.ReadAsStringAsync();
            var s = message.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
                Assert.True(s.Equals(DataHelper.ErrorMessage.NotFoundBookByTitle(book)));
            });
        }

        [Fact]
        public async Task GetBooksByAuthor_Return_OK()
        {
            var response = await _libraryHttpService.GetBooksByAuthor(_book.Author);
            var listStringBooks = await response.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<List<Book>>(listStringBooks);

            Assert.Multiple(() =>
            {
                Assert.Equal(response.StatusCode, HttpStatusCode.OK);
                Assert.NotNull(response);
                Assert.Equal(json[0].Title, _book.Title);
                Assert.Equal(json[0].Author, _book.Author);
                Assert.Equal(json[0].YearOfRelease, _book.YearOfRelease);
            });
        }

        [Fact]
        public async Task GetBookByAuthor_Return_NotFound()
        {
            var book = DataHelper.BookHelper.BookWithTitleAuthorYear("Not Found", "Not Found Author", 1990);
            var response = await _libraryHttpService.GetBooksByAuthor(book.Author);
            var message = await response.Content.ReadAsStringAsync();
            var s = message.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.Equal(response.StatusCode, HttpStatusCode.NotFound);
                Assert.True(s.Equals(DataHelper.ErrorMessage.NotFoundBookByAuthor(book)));
            });
        }

        public async Task DisposeAsync()
        {
            ConsoleHelper.TearDownFromClass(GetType().Name);
            await _libraryHttpService.DeleteBook(_book.Title, _book.Author);
        }
    }
}