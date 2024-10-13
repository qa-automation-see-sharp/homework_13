using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.Fixtures;
using LibraryV4.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api;

public class CreateBookTests : LibraryTestFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Test]
    [Description("This test checks if the book is created successfully")]
    public async Task CreateBookAsync_ReturnOK()
    {
        //Arrange
        var book = DataHelper.CreateBook();

        //Act
        var httpResponseMessage = 
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);

        //Assert
        Assert.Multiple(() =>
        {
            Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(bookFromResponse.Title, Is.EqualTo(book.Title));
            Assert.That(bookFromResponse.Author, Is.EqualTo(book.Author));
            Assert.That(bookFromResponse.YearOfRelease, Is.EqualTo(book.YearOfRelease));
        });
    }

    [Test]
        public async Task CreateExistingBookAsync_ReturnBadRequest()
        {
            //Arrange
            var book = DataHelper.CreateBook();
            
            var httpResponseMessage = 
                await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);
            
            //Act
            var httpResponseMessage2 = 
                await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse);
            
            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(httpResponseMessage2.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(bookFromResponse.Title, Is.EqualTo(book.Title));
                Assert.That(bookFromResponse.Author, Is.EqualTo(book.Author));
                Assert.That(bookFromResponse.YearOfRelease, Is.EqualTo(book.YearOfRelease));
            });
            
        }
}