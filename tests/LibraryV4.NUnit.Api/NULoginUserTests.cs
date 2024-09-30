using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.NUnit.Api.TestHelpers;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;

namespace LibraryV4.NUnit.Tests.Api.Tests
{

    public class LoginUserTests
    {
        private User _testUser;
        private Guid _token = Guid.Empty;

        private LibraryHttpService _libraryHttpService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            await _libraryHttpService.LoginTestUser();
        }

        //Login with correct credentials return Ok
        [Test]
        public async Task Login_IfCredentialsAreCorrect_Return_Ok()
        {
            _testUser = UserHelpers.User_ExistNickName();

            HttpResponseMessage response = await _libraryHttpService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"jsonResult is: {jsonResult}");
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Login with correct NickName and wrong password return BadRequest
        [Test]
        public async Task Login_IfPasswordIsIncorrect_Return_BadRequest()
        {
            _testUser = UserHelpers.User_ExistNickName();
            _testUser.Password = "WrongPassword";
            
            HttpResponseMessage response = await _libraryHttpService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Login with wrong NickName and correct password return BadRequest
        [Test]
        public async Task Login_IfNickNameIsIncorrect_Return_BadRequest()
        {
            _testUser = UserHelpers.User_ExistNickName();
            _testUser.NickName = "WrongNickName";

            HttpResponseMessage response = await _libraryHttpService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Login with wrong NickName and wrong password return BadRequest
        [Test]
        public async Task Login_IfNickNameAndPasswordAreIncorrect_Return_BadRequest()
        {
            _testUser = UserHelpers.CreateUser();
            
            HttpResponseMessage response = await _libraryHttpService.LogIn(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }
    }
}

