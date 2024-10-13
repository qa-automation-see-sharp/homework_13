using System.Net;
using LibraryV4.Fixtures;
using LibraryV4.TestHelpers;

namespace LibraryV4.xUnit.Tests.Api;

public class UserTests : LibraryTestFixture
{
    [Fact]
    public async Task RegisterUserAsync_ReturnCreated()
    {
        // Arrange
        var response = await _libraryHttpService.CreateUser(DataHelper.CreateUser());

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task LogInAsync_ReturnOK()
    {
        // Arrange
        var response = await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, false);
        var token = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrEmpty(token));
    }
}