using System.Net;
using LibraryV4.Common.Files.For.Tests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api.Tests
{
    public class UserTests : Fixture
    {
        private User User { get; set; }

        [OneTimeSetUp]
        public async Task SetUp()
        {
            User = await HttpService.CreateDefaultUser();
        }

        [Test]
        public async Task CreateUserSusses()
        {
            User user = DataHelper.UserHelper.CreateRandomUser();

            var response = await HttpService.CreateUser(user);
            var json = await response.Content.ReadAsStringAsync();
            var u = JsonConvert.DeserializeObject<User>(json);

            Assert.Multiple(() =>
            {
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
                Assert.That(response, Is.Not.Null);
                Assert.That(u.FullName, Is.EqualTo(user.FullName));
                Assert.That(u.NickName, Is.EqualTo(user.NickName));
            });
        }

        [Test]
        public async Task CreateExistesUserBadRequest()
        {
            User user = DataHelper.UserHelper.CreateRandomUser();

            await HttpService.CreateUser(user);
            var response = await HttpService.CreateUser(user);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.That(s.Equals(DataHelper.ErrorMessage.ExistUser(user.NickName)));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public async Task LoginUser()
        {
            var message = await HttpService.LogIn(User);
            var json = await message.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<AuthorizationToken>(json);

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(obj, Is.Not.Null);
                Assert.That(obj.Token, Is.Not.Empty);
            });
        }

        [Test]
        public async Task LoginBadRequest()
        {
            var message = await HttpService.LogIn("", "");
            var json = await message.Content.ReadAsStringAsync();
            var s = json.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.That(message.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(s.Equals(DataHelper.ErrorMessage.InvalidLogin));
            });
        }
    }
}