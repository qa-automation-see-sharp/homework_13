using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using LibraryV4.xUnit.Tests.Api.Tests.TestHelpers;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;


//TODO fix namespace
namespace LibraryV4.xUnit.Tests.Api.Tests
{
    [Collection("Non-Parallel Collection")]
    public class XUCreateBookTests : IAsyncLifetime, IClassFixture<TestFixture>
    {
        private Guid _token = Guid.Empty;
        private Book _book;
        private readonly TestLoggerHelper _logger;
        private LibraryHttpService _libraryService;
        private User _user;

        //setup#1
        public XUCreateBookTests(ITestOutputHelper output, TestFixture fixture)
        {
            fixture.InitializaLogger(output);
            _libraryService = fixture.libraryHttpService;
            _logger = fixture.TestLoggerHelper;
        }

        //setup2
        public async Task InitializeAsync()
        {
            //TODO if this code is not used, should it be here?
            //await _libraryService.CreateTestUser();
            //await _libraryService.LoginTestUser();
        }

        //Tests
        //Create Book without authorization token return unauthorized
        [Fact]
        public async Task CreateBook_IfUnauthorazedUser_Return_Unauthorized()
        {
            //Arrange
            _book = BookHelpers.CreateBook();
            await _libraryService.CreateBook(_token, _book);

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_token, _book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with new Title and new Author return created
        [Fact]
        public async Task CreateBook_IfNewTitleNewAuthor_Return_Created()
        {
            //Arrange
            _book = BookHelpers.CreateBook();
            
            //TODO if this code is not used, should it be here?
            //_logger.LogInformation($"Book: {_book.Title}, {_book.Author}, {_book.YearOfRelease}");

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with existing Title and existing Author return conflict
        [Fact]
        public async Task CreateBook_IfExistTitleExistAuthor_Return_BadRequest()
        {
            //Arrange

            _book = BookHelpers.CreateExistBook();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with existing Title and new Author return created
        [Fact]
        public async Task CreateBook_IfExistTitleNewAuthor_Return_Created()
        {
            //Arrange
            _book = BookHelpers.CreateExistBook();
            _book.Author = "Test Author" + Guid.NewGuid();

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");

            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Create Book with new Title and existing Author return created
        [Fact]
        public async Task CreateBook_IfNewTitleExistAuthor_Return_Created()
        {
            //Arrange

            _book = BookHelpers.CreateBook();
            _book.Title = "Test Book" + Guid.NewGuid().ToString();
            _book.Author = "Test Author";

            //Act
            HttpResponseMessage response = await _libraryService.CreateBook(_book);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newBook = JsonConvert.DeserializeObject<Book>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(jsonResult);
                Assert.Equal(newBook.Title, _book.Title);
                Assert.Equal(newBook.Author, _book.Author);
                Assert.Equal(newBook.YearOfRelease, _book.YearOfRelease);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");

            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Teardown
        public Task DisposeAsync()
        {
            //Act
            return Task.CompletedTask;
        }
    }
}
