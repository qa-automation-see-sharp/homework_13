using System.Net;
using DocsForTests.TestHelpers;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api.Tests
{
    public class CreateUserTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
    {
        private readonly LibraryHttpService _libraryHttpService;
        private User User;

        public CreateUserTests(LibraryHttpService libraryHttpService)
        {
            _libraryHttpService = libraryHttpService;
        }
        public async Task InitializeAsync()
        {
            ConsoleHelper.SetUpFromClass(GetType().Name);
        }

        [Fact]
        public async Task CreateUser_ReturnCreated()
        {
            User user = DataHelper.UserHelper.CreateRandomUser();

            var response = await _libraryHttpService.CreateUser(user);
            var json = await response.Content.ReadAsStringAsync();
            var u = JsonConvert.DeserializeObject<User>(json);

            Assert.Multiple(() =>
            {
                Assert.Equal(response.StatusCode, HttpStatusCode.Created);
                Assert.NotNull(response);
                Assert.Equal(u.FullName, user.FullName);
                Assert.Equal(u.NickName, user.NickName);
            });
        }

        [Fact]
        public async Task CreateExistesUser_ReturnBadRequest()
        {
            User user = DataHelper.UserHelper.CreateRandomUser();

            await _libraryHttpService.CreateUser(user);
            var response = await _libraryHttpService.CreateUser(user);
            var jsonString = await response.Content.ReadAsStringAsync();
            var s = jsonString.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.True(s.Equals(DataHelper.ErrorMessage.ExistUser(user.NickName)));
                Assert.Equal(response.StatusCode, HttpStatusCode.BadRequest);
            });
        }

        [Fact]
        public async Task LoginUser_ReturnOK()
        {
            var message = await _libraryHttpService.LogIn(User);
            var json = await message.Content.ReadAsStringAsync();
            var obj = JsonConvert.DeserializeObject<AuthorizationToken>(json);

            Assert.Multiple(() =>
            {
                Assert.Equal(message.StatusCode, HttpStatusCode.OK);
                Assert.NotNull(obj);
                Assert.NotEmpty(obj.Token.ToString());
            });
        }

        [Fact]
        public async Task LoginUser_ReturnBadRequest()
        {
            var message = await _libraryHttpService.LogIn("", "");
            var json = await message.Content.ReadAsStringAsync();
            var s = json.Trim('"');

            Assert.Multiple(() =>
            {
                Assert.Equal(message.StatusCode, HttpStatusCode.BadRequest);
                Assert.True(s.Equals(DataHelper.ErrorMessage.InvalidLogin));
            });
        }

        public async Task DisposeAsync()
        {
            ConsoleHelper.TearDownFromClass(GetType().Name);
        }
    }
}