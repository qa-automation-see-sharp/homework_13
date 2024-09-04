using System.Net;
using LibraryV4.NUnit.Api.Fixtures;
using Newtonsoft.Json;
using static LibraryV4.NUnit.Api.TestHelpers.DataHelper;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.NUnit.Api.Tests;

public class GetBooksTests : LibraryV4TestFixture
{

    [Test]
    public async Task GetBooksByTitle_ShouldReturnOK()
    {
        //Arrange
        var book = CreateBook();
        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.GetBooksByTitle(book.Title);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(bookFromResponse[0].Title, Is.EqualTo(book.Title));
            Assert.That(bookFromResponse[0].Author, Is.EqualTo(book.Author));
            Assert.That(bookFromResponse[0].YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task GetBooksByTitle_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var title = Guid.NewGuid().ToString() + "additional characters";

        //Act
        var response = await LibraryHttpService.GetBooksByTitle(title);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }

    [Test]
    public async Task GetBooksByAuthor_ShouldReturnOK()
    {
        //Arrange
        var book = CreateBook();
        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.GetBooksByAuthor(book.Author);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<List<Book>>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(bookFromResponse[0].Title, Is.EqualTo(book.Title));
            Assert.That(bookFromResponse[0].Author, Is.EqualTo(book.Author));
            Assert.That(bookFromResponse[0].YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task GetBooksByAuthor_BookDoesNotExist_ShouldReturnNotFound()
    {
        //Arrange
        var author = Guid.NewGuid().ToString() + "additional characters";

        //Act
        var response = await LibraryHttpService.GetBooksByAuthor(author);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}