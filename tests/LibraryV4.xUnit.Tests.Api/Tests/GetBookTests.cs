using System.Net;
using LibraryV4.xUnit.Tests.Api.Services;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using Newtonsoft.Json;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.xUnit.Tests.Api.Tests;

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
        //Arrange
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
        _book = DataHelper.CreateBook();
        await _libraryHttpService.PostBook(_book);
    }

    [Fact]
    public async Task GetBooksByTitle_ShouldReturnOK()
    {
        //Act
        var response = await _libraryHttpService.GetBooksByTitle(_book.Title);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_book.Title, bookFromResponse[0].Title);
            Assert.Equal(_book.Author, bookFromResponse[0].Author);
            Assert.Equal(_book.YearOfRelease, bookFromResponse[0].YearOfRelease);
        });
    }

    [Fact]
    public async Task GetBooksByTitle_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var title = Guid.NewGuid().ToString() + "additional characters";

        //Act
        var response = await _libraryHttpService.GetBooksByTitle(title);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetBooksByAuthor_ShouldReturnOK()
    {
        //Act
        var response = await _libraryHttpService.GetBooksByAuthor(_book.Author);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(_book.Title, bookFromResponse[0].Title);
            Assert.Equal(_book.Author, bookFromResponse[0].Author);
            Assert.Equal(_book.YearOfRelease, bookFromResponse[0].YearOfRelease);
        });
    }

    [Fact]
    public async Task GetBooksByAuthor_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var author = Guid.NewGuid().ToString() + "additional characters";

        //Act
        var response = await _libraryHttpService.GetBooksByAuthor(author);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    public async Task DisposeAsync()
    {
        await _libraryHttpService.DeleteBook(_book.Title, _book.Author);
    }
}