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
    public class XULoginUserTests : IAsyncLifetime, IClassFixture<TestFixture>
    {
        private User _loginTestUser;
        private readonly TestLoggerHelper _logger;

        private LibraryHttpService _libraryService;

        //SetUp #1
        public XULoginUserTests(TestFixture fixture, ITestOutputHelper output)
        {
            fixture.InitializaLogger(output);
            _libraryService = fixture.libraryHttpService;
            _logger = fixture.TestLoggerHelper;
        }

        //setup#2
        public async Task InitializeAsync()
        {
            //Arrange
            //await _libraryService.CreateTestUser();
            //await _libraryService.LoginTestUser();
        }

        //Login with correct credentials return Ok
        [Fact]
        public async Task IfCredentialsAreCorrect_Return_Ok()
        {
            //Arrange
            _loginTestUser = UserHelpers.LoginUser();

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_loginTestUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            //_logger.LogInformation($"jsonResult is: {jsonResult}");
            var newUser = JsonConvert.DeserializeObject<AuthorizationToken>(jsonResult);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotNull(jsonResult);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {jsonResult}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {jsonResult}");
        }

        //Login with correct NickName and wrong password return BadRequest
        [Fact]
        public async Task IfNickNameIsCorrectAndPasswordIsWrong_Return_BadRequest()
        {
            //Arrange
            _loginTestUser = UserHelpers.LoginUser();
            _loginTestUser.Password = "WrongPassword";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_loginTestUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

            //Asserts
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

        //Login with wrong NickName and correct password return BadRequest
        [Fact]
        public async Task IfNickNameIsWrongAndPasswordIsCorrect_Return_BadRequest()
        {
            //Arrange
            _loginTestUser = UserHelpers.LoginUser();
            _loginTestUser.NickName = "WrongNickName";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_loginTestUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

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

        //Login with wrong NickName and wrong password return BadRequest
        [Fact]
        public async Task IfNickNameIsWrongAndPasswordIsWrong_Return_BadRequest()
        {
            //Arrage
            _loginTestUser = UserHelpers.LoginUser();
            _loginTestUser.NickName = "WrongNickName";
            _loginTestUser.Password = "WrongPassword";

            //Act
            HttpResponseMessage response = await _libraryService.LogIn(_loginTestUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");

            //Asserts
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

        //TearDown
        public Task DisposeAsync()
        {
            //Act
            return Task.CompletedTask;
        }
    }
}
