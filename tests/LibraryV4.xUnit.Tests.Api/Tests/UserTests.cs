using Bogus;
using LibraryV4.Contracts.Domain;
using LibraryV4.Tests.Services.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV4.xUnit.Tests.Api.Tests;

public class UsersTests : IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    public UsersTests(LibraryHttpService libraryHttpService) { _libraryHttpService = libraryHttpService; }


    private User GenerateUser()
    {
        var faker = new Faker();

        return new User
        {
            FullName = "David Solis",
            NickName = $"soledavi{faker.Random.AlphaNumeric(4)}",
            Password = "126rtgc"
        };
    }

    [Fact]
    public async Task CreateUser_WhenDataIsValid_ReturnCreated()
    {
        var user = GenerateUser();
        HttpResponseMessage response = await _libraryHttpService.CreateUser(user);

        var jsonString = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<User>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.Created);
            Assert.Equal(users.FullName, user.FullName);
            Assert.Equal(users.NickName, user.NickName);
        });
    }

    [Fact]
    public async Task LogIn_WhenUserExists_ReturnOk()
    {
        var user = GenerateUser();

        await _libraryHttpService.CreateUser(user);

        HttpResponseMessage response = await _libraryHttpService.LogIn(user);

        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.NotNull(jsonString);
        });
    }
}