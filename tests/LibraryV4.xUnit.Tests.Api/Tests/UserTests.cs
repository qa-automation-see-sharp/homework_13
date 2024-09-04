using System.Net;
using LibraryV4.xUnit.Tests.Api.Services;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using Newtonsoft.Json;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.xUnit.Tests.Api.Tests;

public class UsersTests : IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;      

    public UsersTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }

    [Fact]
    public async Task CreateUser_ShouldReturnCreated()
    {
        
        //Arrange
        var user = DataHelper.CreateUser();

        //Act
        var httpResponseMessage = await _libraryHttpService.CreateUser(user);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<User>(content);

        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.Equal(user.FullName, response.FullName);
            Assert.Equal(user.NickName, response.NickName);
        });
    }

    [Fact]
    public async Task CreateUser_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Act
        var httpResponseMessage = await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
    }

    [Fact]
    public async Task Login_ShouldReturnOK()
    {
        
        //Arrange        
        await _libraryHttpService.CreateUser(_libraryHttpService.DefaultUser);

        //Act
        var httpResponseMessage = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var response = JsonConvert.DeserializeObject<AuthorizationToken>(content);

        //Assert
        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
    }

    [Fact]
    public async Task Login_UserDoesNotExist_ShouldReturnBadRequest()
    {
        //Arrange
        var user = DataHelper.CreateUser();

        //Act
        var httpResponseMessage = await _libraryHttpService.LogIn(user);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
    }
}