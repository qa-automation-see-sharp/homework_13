using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.NUnit.Api.TestHelpers;

namespace LibraryV4.NUnit.Tests.Api.Tests
{

    public class CreateUserTests
    {
        private User _testUser;
        private Guid _token = Guid.Empty;
        private LibraryHttpService _libraryHttpService;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            _libraryHttpService = new LibraryHttpService();
            await _libraryHttpService.CreateTestUser();
            
        }

        [Test]
        public async Task CreateUser_IfUserDoesnotExist_Return_Created()
        {
            _testUser = UserHelpers.CreateUser();
            
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newUser.FullName, Is.EqualTo(_testUser.FullName));
                Assert.That(newUser.NickName, Is.EqualTo(_testUser.NickName));
            });
        }

        [Test]
        public async Task CreateUser_IfUserExist_Return_BadRequest()
        {
            _testUser = UserHelpers.User_ExistNickName();
            
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc nickname return BadRequest
        [Test]
        public async Task CreateUser_IfNickNameIsBlank_Return_BadRequest()
        {
            _testUser = UserHelpers.CreateUser();
            _testUser.NickName = ""; //Blank NickName
            
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with spaced nickname return BadRequest
        [Test]
        public async Task CreateUser_IfNickNameIsSpaced_Return_BadRequest()
        {
            _testUser = UserHelpers.CreateUser();
            _testUser.NickName = "   "; //Spaced NickName
            
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc password return BadRequest
        [Test]
        public async Task CreateUser_IfPasswordIsBlank_Return_BadRequest()
        {
            _testUser = UserHelpers.CreateUser();
            _testUser.Password = ""; //Blank Password
            
            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(jsonResult, Is.Not.Null);
            });
        }

        //Create a new user with blanc fullname return Created
        [Test]
        public async Task CreateUser_IfFullNameIsBlank_Return_Created()
        {
            _testUser = UserHelpers.CreateUser();
            _testUser.FullName = ""; //Blank FullName

            HttpResponseMessage response = await _libraryHttpService.CreateUser(_testUser);
            var jsonResult = await response.Content.ReadAsStringAsync();
            var newUser = JsonConvert.DeserializeObject<User>(jsonResult);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(jsonResult, Is.Not.Null);
                Assert.That(newUser.FullName, Is.EqualTo(_testUser.FullName));
                Assert.That(newUser.NickName, Is.EqualTo(_testUser.NickName));
            });
        }

    }
}

