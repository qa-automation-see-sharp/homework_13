using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using TestUtils;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api;

public class CreateBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    
    public CreateBookTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }
    
    public async Task InitializeAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Fact]
    public async Task CreateBookAsync_ReturnOK()
    {
        // Arrange
        var book = DataHelper.CreateBook();

        // Act
        var httpResponseMessage =
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);

        // Assert
        Assert.Equal(HttpStatusCode.Created, httpResponseMessage.StatusCode);
        Assert.Equal(book.Title, bookFromResponse.Title);
        Assert.Equal(book.Author, bookFromResponse.Author);
        Assert.Equal(book.YearOfRelease, bookFromResponse.YearOfRelease);
    }

    [Fact]
    public async Task CreateExistingBookAsync_ReturnBadRequest()
    {
        // Arrange
        var book = DataHelper.CreateBook();

        var httpResponseMessage =
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);

        // Act
        var httpResponseMessage2 =
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, httpResponseMessage2.StatusCode);
        Assert.Equal(book.Title, bookFromResponse.Title);
        Assert.Equal(book.Author, bookFromResponse.Author);
        Assert.Equal(book.YearOfRelease, bookFromResponse.YearOfRelease);
    }
    
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}