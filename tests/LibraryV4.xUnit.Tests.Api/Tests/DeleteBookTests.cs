using LibraryV4.Contracts.Domain;
using LibraryV4.Tests.Services.Services;
using System.Net;

namespace LibraryV4.xUnit.Tests.Api.Tests;

public class DeleteBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService; 
    public DeleteBookTests(LibraryHttpService libraryHttpService) { _libraryHttpService = libraryHttpService; }

    public async Task InitializeAsync()
    {
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
    }

    [Fact]
    public async Task DeleteBook_WhenBookExists_ReturnOk()
    {
        var newBook = new Book()
        {
            Title = "ToDelete",
            Author = "None"
        };

        var bookCreated = await _libraryHttpService.CreateBook(newBook);

        HttpResponseMessage response = await _libraryHttpService.DeleteBook(newBook.Title, newBook.Author);

        var jsonString = await response.Content.ReadAsStringAsync();

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.True(jsonString.Contains("ToDelete by None deleted"));
        });
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}