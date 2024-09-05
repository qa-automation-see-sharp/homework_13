using LibraryV4.Contracts.Domain;
using LibraryV4.Tests.Services.Services;
using System.Net;

namespace LibraryV4.NUnit.Tests.Api.Tests;

public class DeleteBookTests
{
    private LibraryHttpService _libraryHttpService;

    [SetUp]
    public async Task SetUp()
    {
        _libraryHttpService = new LibraryHttpService();
        await _libraryHttpService.CreateDefaultUser();
        await _libraryHttpService.Authorize();
    }

    [Test]
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
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(jsonString.Contains("ToDelete by None deleted"));
        });
    }
}