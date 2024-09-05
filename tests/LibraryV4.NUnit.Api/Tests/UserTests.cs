using Bogus;
using LibraryV4.Contracts.Domain;
using LibraryV4.Tests.Services.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV4.NUnit.Tests.Api.Tests;

public class UsersTests
{
    private LibraryHttpService _libraryHttpService;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _libraryHttpService = new LibraryHttpService();
    }

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

    [Test]
    public async Task CreateUser_WhenDataIsValid_ReturnCreated()
    {
        var user = GenerateUser();
        HttpResponseMessage response = await _libraryHttpService.CreateUser(user);

        var jsonString = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<User>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(users.FullName, Is.EqualTo(user.FullName));
            Assert.That(users.NickName, Is.EqualTo(user.NickName));
        });
    }

    [Test]
    public async Task LogIn_WhenUserExists_ReturnOk()
    {
        var user = GenerateUser();

        await _libraryHttpService.CreateUser(user);

        HttpResponseMessage response = await _libraryHttpService.LogIn(user);

        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(jsonString, Is.Not.Null);
        });
    }
}