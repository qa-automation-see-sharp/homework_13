using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.NUnit.Tests.Api.Fixtures;
using LibraryV4.NUnit.Tests.Api.Services;
using Newtonsoft.Json;
using static LibraryV4.NUnit.Tests.Api.TestHelpers.DataHelper;

namespace LibraryV3.NUnit.Tests.Api.Tests;

public class UsersTests : LibraryV4TestFixture
{
    [Test]
    public async Task CreateUser_ShouldReturnCreated()
    {
        //Arrange
        var user = CreateUser();

        //Act
        var httpResponseMessage = await LibraryHttpService.CreateUser(user);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<User>(content);

        Assert.Multiple(() =>
        {
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.FullName, Is.EqualTo(user.FullName));
            Assert.That(response.NickName, Is.EqualTo(user.NickName));
        });
    }

    [Test]
    public async Task CreateUser_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var user = CreateUser();
        await LibraryHttpService.CreateUser(user);

        //Act
        var httpResponseMessage = await LibraryHttpService.CreateUser(user);

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task Login_ShouldReturnOK()
    {

        //Arrange
        var user = CreateUser();
        await LibraryHttpService.CreateUser(user);

        //Act
        var httpResponseMessage = await LibraryHttpService.LogIn(user);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<AuthorizationToken>(content);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            //Assert.That(response.Token, Is.Not.Null);
           // Assert.That(response.NickName, Is.EqualTo(user.NickName));
        });
    }

    [Test]
    public async Task Login_UserDoesNotExist_ShouldReturnBadRequest()
    {
        //Arrange
        var user = CreateUser();

        //Act
        var httpResponseMessage = await LibraryHttpService.LogIn(user);

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}