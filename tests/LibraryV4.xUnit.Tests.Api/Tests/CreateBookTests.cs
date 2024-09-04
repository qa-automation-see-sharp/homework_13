using System.Net;
using LibraryV4.xUnit.Tests.Api.Services;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using Newtonsoft.Json;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.xUnit.Tests.Api.Tests;

public sealed class CreateBookTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;   

    public CreateBookTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }

    public async Task InitializeAsync()
    {   
        //Arrange
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();        
    }

    [Fact]
    public async Task PostBook_ShouldReturnCreated()
    {
        //Arrange
        var book = DataHelper.CreateBook(); 
        
        //Act
        var response = await _libraryHttpService.PostBook(book);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var createdBook = JsonConvert.DeserializeObject<Book>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            Assert.Equal(book.Title, createdBook.Title);
            Assert.Equal(book.Author, createdBook.Author);
            Assert.Equal(book.YearOfRelease, createdBook.YearOfRelease);
        });
    }

    [Fact]
    public async Task PostBook_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var book = DataHelper.CreateBook(); 
        await _libraryHttpService.PostBook(book);

        //Act
        var response = await _libraryHttpService.PostBook(book);

        //Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task PostBook_ShouldReturnUnauthorized()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Arrange
        var httpResponseMessage = await _libraryHttpService.PostBook(Guid.NewGuid().ToString(), book);

        //Assert
        Assert.Equal(HttpStatusCode.Unauthorized, httpResponseMessage.StatusCode);
    }

    public async Task DisposeAsync()
    {
    }
}