using Bogus;
using LibraryV4.Contracts.Domain;
using Newtonsoft.Json;
using System.Net;
using LibraryV4.Tests.Services.Services;

namespace LibraryV4.xUnit.Tests.Api.Tests;

public class GetBooksTests : IAsyncLifetime, IClassFixture<LibraryHttpService>
{
    private readonly LibraryHttpService _libraryHttpService;
    private Book _newBook;
    public GetBooksTests(LibraryHttpService libraryHttpService) { _libraryHttpService = libraryHttpService; }

    public async Task InitializeAsync()
    {
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();

        var faker = new Faker();
        _newBook = new Book
        {
            Author = "Kotaro Isaka",
            Title = $"Grasshopper {faker.Random.AlphaNumeric(4)}",
            YearOfRelease = 2004
        };

        await _libraryHttpService.CreateBook(_newBook);
    }

    [Fact]
    public async Task GetBooksByTitle_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(_newBook.Title);

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.True(books.Count > 0);
            Assert.Equal(books[0].Title, _newBook.Title);
            Assert.Equal(books[0].Author, "Kotaro Isaka");
            Assert.Equal(books[0].YearOfRelease, 2004);
        });
    }

    [Fact]
    public async Task GetBooksByAuthor_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor("Kotaro Isaka");

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.Equal(response.StatusCode, HttpStatusCode.OK);
            Assert.True(books.Count >0);
            Assert.Equal(books[0].Title, _newBook.Title);
            Assert.Equal(books[0].Author, "Kotaro Isaka");
            Assert.Equal(books[0].YearOfRelease, 2004);
        });
    }

    public new async Task DisposeAsync()
    {
        await _libraryHttpService.DeleteBook(_newBook.Title, _newBook.Author);
    }
}