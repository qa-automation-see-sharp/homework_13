using System.Net;
using LibraryV4.NUnit.Api.Fixtures;
using Newtonsoft.Json;
using static LibraryV4.NUnit.Api.TestHelpers.DataHelper;
using LibraryV4.Contracts.Domain;

namespace LibraryV4.NUnit.Api.Tests;

public sealed class CreateBookTests : LibraryV4TestFixture
{
    [Test]
    public async Task PostBook_ShouldReturnCreated()
    {
        //Arrange
        var book = CreateBook(); 
        
        //Act
        var response = await LibraryHttpService.PostBook(book);
        var bookJsonString = await response.Content.ReadAsStringAsync();
        var createdBook = JsonConvert.DeserializeObject<Book>(bookJsonString);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(createdBook.Title, Is.EqualTo(book.Title));
            Assert.That(createdBook.Author, Is.EqualTo(book.Author));
            Assert.That(createdBook.YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
    public async Task PostBook_AlreadyExists_ShouldReturnBadRequest()
    {
        //Arrange
        var book = CreateBook();    

        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.PostBook(book);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }

    [Test]
    public async Task PostBook_ShouldReturnUnauthorized()
    {
        //Arrange
        var book = CreateBook();

        //Arrange
        var httpResponseMessage = await LibraryHttpService.PostBook(Guid.NewGuid().ToString(), book);

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}