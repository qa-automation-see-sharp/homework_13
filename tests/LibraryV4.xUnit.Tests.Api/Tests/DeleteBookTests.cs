using System.Net;
using LibraryV4.Common.Files.For.Tests.TestHelpers;
using LibraryV4.Contracts.Domain;
using static LibraryV4.Common.Files.For.Tests.TestHelpers.DataHelper;

namespace LibraryV4.xUnit.Tests.Api.Tests
{
    public class DeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private readonly LibraryHttpService _libraryHttpService;
        private Book _book;

        public DeleteBookTests(LibraryHttpService libraryHttpService){
            _libraryHttpService = libraryHttpService;
        }

        public async Task InitializeAsync()
        {
            ConsoleHelper.SetUpFromClass(GetType().Name);
            _book = BookHelper.RandomBook();
            await _libraryHttpService.PostBook(_book);
        }

        [Fact]
        public async Task DeleteBook_ReturnOK()
        {
            var response = await _libraryHttpService.DeleteBook(_book.Title, _book.Author);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.Equal(ErrorMessage.DeleteBookReturnOK(_book), s);
                Assert.True(response.StatusCode == HttpStatusCode.OK);
            });
        }

        [Fact]
        public async Task DeleteBook_ReturnNotFound()
        {
            var title = "Happy";
            var author = "Chan";
            var response = await _libraryHttpService.DeleteBook(title, author);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.Equal(ErrorMessage.DeleteBookNotFound(title, author), s);
                Assert.True(response.StatusCode == HttpStatusCode.NotFound);
            });
        }

        [Fact]
        public async Task DeleteBook_ReturnUnauthorized()
        {
            var response = await _libraryHttpService.DeleteBook(_book.Title, _book.Author, "123");

            Assert.True(response.StatusCode == HttpStatusCode.Unauthorized);
        }

        public async Task DisposeAsync()
        {
            ConsoleHelper.TearDownFromClass(GetType().Name);
        }
    }
}