using LibraryV4.Contracts.Domain;
using LibraryV4.xUnit.Tests.Api.TestHelpers;
using LibraryV4.xUnit.Tests.Api.Services;
using LibraryV4.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV4.NUnit.Tests.Api.Tests;

public class GetBooksTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    private Book _newBook;
    public GetBooksTests(LibraryHttpService libraryHttpService)
    {
        _libraryHttpService = libraryHttpService;
    }

    public async Task InitializeAsync()
    {
        //Await
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
        _newBook=DataHelper.CreateBook();
        await _libraryHttpService.PostBook(_newBook);

        _newBook = new Book
        {
            Author = "Test Author",
            Title = $"Test Title",
            YearOfRelease = 2000
        };

        await _libraryHttpService.PostBook(_newBook);
    }

    [Fact]
    public async Task GetBooksByTitle_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(_newBook.Title);

        var bookjsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(bookjsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.Equal(books[0].Title, _newBook.Title);
            Assert.Equal(books[0].Author, "Test Author");
            Assert.Equal(books[0].YearOfRelease, 2000);
        });
    }

    [Fact]
    public async Task GetBooksByAuthor_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor("Test Author");

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.True(books.Count > 0);
            Assert.Equal(books[0].Title, _newBook.Title);
            Assert.Equal(books[0].Author, "Test Author");
            Assert.Equal(books[0].YearOfRelease, 2000);
        });
    }
    [Fact]
    public async Task GetBooksByAuthor_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var author = Guid.NewGuid().ToString();

        //Act
        var response = await _libraryHttpService.GetBooksByAuthor(author);

        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    public new async Task DisposeAsync()
    {
        await _libraryHttpService.DeleteBook(_newBook.Title, _newBook.Author);
    }
}