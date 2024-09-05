using Bogus;
using LibraryV4.Contracts.Domain;
using LibraryV4.Tests.Services.Services;
using Newtonsoft.Json;
using System.Net;

namespace LibraryV4.NUnit.Tests.Api.Tests;

[TestFixture]
public class GetBooksTests
{
    private LibraryHttpService _libraryHttpService;
    private Book _newBook;

    [OneTimeSetUp]
    public async Task SetUp()
    {
        _libraryHttpService = new LibraryHttpService();
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

    [Test]
    public async Task GetBooksByTitle_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByTitle(_newBook.Title);

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(books.Count, Is.GreaterThan(0));
            Assert.That(books[0].Title, Is.EqualTo(_newBook.Title));
            Assert.That(books[0].Author, Is.EqualTo("Kotaro Isaka"));
            Assert.That(books[0].YearOfRelease, Is.EqualTo(2004));
        });
    }

    [Test]
    public async Task GetBooksByAuthor_WhenBookExists_ReturnOk()
    {
        HttpResponseMessage response = await _libraryHttpService.GetBooksByAuthor("Kotaro Isaka");

        var jsonString = await response.Content.ReadAsStringAsync();

        var books = JsonConvert.DeserializeObject<List<Book>>(jsonString);

        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(books.Count, Is.GreaterThan(0));
            Assert.That(books[0].Title, Is.EqualTo(_newBook.Title));
            Assert.That(books[0].Author, Is.EqualTo("Kotaro Isaka"));
            Assert.That(books[0].YearOfRelease, Is.EqualTo(2004));
        });
    }

    [OneTimeTearDown]
    public new async Task OneTimeTearDown()
    {
        await _libraryHttpService.DeleteBook(_newBook.Title, _newBook.Author);
    }
}