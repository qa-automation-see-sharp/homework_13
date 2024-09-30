using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using LibraryV4.xUnit.Tests.Api.Tests.TestHelpers;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;

namespace LibraryV4.xUnit.Tests.Api.Tests
{
    [Collection("Non-Parallel Collection")]
    public class XUGetBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>, IClassFixture<TestFixture>
    {
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;
        private Book _book;
        private readonly TestFixture _testFixture;

        //setup1
        public XUGetBookTests(LibraryHttpService libraryHttpService, ITestOutputHelper output)
        {
            _libraryService = libraryHttpService;
            _logger = new TestLoggerHelper(output);
        }

        //setup2
        public async Task InitializeAsync()
        {
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();

            _book = new Book
            {
                Title = "The Book 1",
                Author = "The Author 1",
                YearOfRelease = 2021
            };
            await _libraryService.CreateBook(_book);
            _logger.LogInformation($"Book: {_book.Title}");
        }

        [Fact]
        public async Task GetBookByTitle_IfTitleExist_Return_Ok()
        {
            //Arrage
            var testTitle = _book.Title;
            //_logger.LogInformation($"Test Title: {testTitle}");

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByTitle(testTitle);
            //_logger.LogInformation($"Response: {response}");
            var jsonResult = await response.Content.ReadAsStringAsync();
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.NotNull(books);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByTitle_IfTitleNotExist_Return_NotFound()
        {
            //Arrange
            var testTitle = "WrongTitle";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByTitle(testTitle);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByAuthor_IfAuthorExist_Return_Ok()
        {
            //Arrange
            var testAuthor = _book.Author;
            _logger.LogInformation($"Test Author: {testAuthor}");

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByAuthor(testAuthor);
            _logger.LogInformation($"Response: {response}");
            var jsonResult = await response.Content.ReadAsStringAsync();
            _logger.LogInformation($"jsonResult: {jsonResult}");
            var books = JsonConvert.DeserializeObject<List<Book>>(jsonResult);
            _logger.LogInformation($"Books: {books}");

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.NotNull(books);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        [Fact]
        public async Task GetBookByAuthor_IfAuthorNotExist_Return_NotFound()
        {
            //Arrange
            var testAuthor = "WrongAuthor";

            //Act
            HttpResponseMessage response = await _libraryService.GetBooksByAuthor(testAuthor);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //TearDown
        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}
