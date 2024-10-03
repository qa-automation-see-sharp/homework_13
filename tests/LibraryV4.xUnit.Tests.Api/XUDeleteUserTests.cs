using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.xUnit.Tests.Api.Tests.TestHelpers;
using System.Net;
using Xunit.Abstractions;

//TODO fix namespace
namespace LibraryV4.xUnit.Tests.Api.Tests
{
    [Collection("Non-Parallel Collection")]
    public class XUDeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private string _token = string.Empty;
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;
        private Book _book;

        //Setup#1
        public XUDeleteBookTests(LibraryHttpService libraryHttpServise, ITestOutputHelper output)
        {
            _libraryService = new LibraryHttpService();
            _logger = new TestLoggerHelper(output);
        }

        //Setup#2
        public async Task InitializeAsync()
        {
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
            _book = new Book()
            {
                Title = "The Book 1",
                Author = "The Author 1",
                YearOfRelease = 2021
            };
            await _libraryService.CreateBook(_book);
        }

        //Tests
        [Fact]
        public async Task DeleteBook_IfExistInLibrary_return_Ok()
        {
            //Arrange
            var book = _book;
            _logger.LogInformation($"Book: {book}");

            //Act
            HttpResponseMessage response = await _libraryService.DeleteBook(book.Title, book.Author);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResponse);
            }
            );
        }

        [Fact]
        public async Task DeleteBook_IfNotExistInLibrary_return_NotFound()
        {
            //Arrange
            var book = BookHelpers.CreateBook();

            //Act
            HttpResponseMessage response = await _libraryService.DeleteBook(book.Title, book.Author);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResponse);
            }
            );
        }

        //TODO if this code is not used, should it be here?
        //Teardown
        public async Task DisposeAsync()
        {
            //Act
        }

    }

}
