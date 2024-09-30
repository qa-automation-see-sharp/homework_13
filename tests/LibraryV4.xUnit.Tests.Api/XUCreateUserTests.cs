using System;
using LibraryV4.Services;
using Xunit.Abstractions;
using Newtonsoft.Json;
using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.xUnit.Tests.Api.Tests.TestHelpers;


namespace LibraryV4.xUnit.Tests.Api.Tests
{
    [Collection("Non-Parallel Collection")]
    public class XUCreateUserTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private LibraryHttpService _libraryService;
        private User _user;
        private string _token = string.Empty;
        private readonly TestLoggerHelper _logger;

        //SetUp #1
        public XUCreateUserTests(LibraryHttpService libraryHttpService, ITestOutputHelper output)
        {

            _libraryService = libraryHttpService;
            _logger = new TestLoggerHelper(output);
        }

        //SetUp #2
        public async Task InitializeAsync()
        {
            //Arrange
            await _libraryService.CreateTestUser();
            await _libraryService.LoginTestUser();
        }

        //Create new user if does not exist return created
        [Fact]
        public async Task CreateNewUser_IfUserNotExist_Return_Created()
        {

            //Arrange
            _user = UserHelpers.CreateUser();

            //Act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(userJsonString);

            //Assert
            Assert.NotNull(newUser);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");

        }

        //Create new user if exist return bad request
        [Fact]
        public async Task CreateNewUser_IfUserExist_Return_BadRequest()
        {
            //Arrange
            _user = UserHelpers.User_ExistNickName();

            //Act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(userJsonString);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");

        }
        //Create user with blank name return bad request
        [Fact]
        public async Task CreateNewUser_IfNickNameBlanc_Return_BadRequest()
        {
            //Arrange
            _user = UserHelpers.CreateUser();
            _user.NickName = "";//Blank NickName

            //Act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(userJsonString);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");

        }

        //Create user with spaced name return bad request CreateNewUser_IfUserExist_Return_BadRequest
        [Fact]
        public async Task CreateNewUser_IfNickNameSpaced_Return_BadRequest()
        {
            //Arrange
            _user = UserHelpers.CreateUser();
            _user.NickName = "   ";//Spaced NickName
            //Act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(userJsonString);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");
        }
        //Create user if password blank return bad request 
        [Fact]
        public async Task CreateNewUser_IfPasswordBlanc_Return_BadRequest()
        {
            //Arrange
            _user = UserHelpers.CreateUser();
            _user.Password = "";//Blank Password

            //act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();

            //Asserts
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
                Assert.NotNull(userJsonString);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");
        }
        //Create user if full name blank return created 
        [Fact]
        public async Task CreateNewUser_IfFullNameBlanc_Return_Created()
        {
            //Arrage
            _user = UserHelpers.CreateUser();
            _user.FullName = "";//Blank FullName

            //Act
            var response = await _libraryService.CreateUser(_user);
            var userJsonString = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(userJsonString);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.Equal(HttpStatusCode.Created, response.StatusCode);
                Assert.NotNull(userJsonString);
                Assert.Equal(_user.FullName, newUser.FullName);
                Assert.Equal(_user.NickName, newUser.NickName);
            });

            //Log output to console
            LoggerHelper.Logger.Information($"Response Status Code: {response.StatusCode}");
            LoggerHelper.Logger.Information($"User JSON String: {userJsonString}");


            //Log output to test output
            _logger.LogInformation($"Response Status Code: {response.StatusCode}");
            _logger.LogInformation($"User JSON String: {userJsonString}");
        }
        //TearDown
        public async Task DisposeAsync()
        {
            //Act
        }
    }
}
