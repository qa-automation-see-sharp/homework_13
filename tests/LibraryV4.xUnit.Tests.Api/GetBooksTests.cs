using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.Services;
using LibraryV4.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api;

public class GetBooksTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    
    private Book _book;

    public GetBooksTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }
    
    public async Task InitializeAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Fact]
    public async Task GetBookByTitleAsync_ReturnOK()
    {
        await CreateBook();
        // Act
        var httpResponseMessage = await _libraryHttpService.GetBooksByTitle(_book.Title);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
    }

    [Fact]
    public async Task GetBookByAuthorAsync_ReturnOK()
    {
        await CreateBook();
        // Act
        var httpResponseMessage = await _libraryHttpService.GetBooksByAuthor(_book.Author);

        // Assert
        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
    }

    private async Task CreateBook()
    {
        var book = DataHelper.CreateBook();

        var httpResponseMessage =
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        _book = JsonConvert.DeserializeObject<Book>(content);
    }
    
    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}