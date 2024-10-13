using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.Fixtures;
using LibraryV4.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV4.NUnit.Api;

public class DeleteBooksTests : LibraryTestFixture
{
    [OneTimeSetUp]
    public async Task OneTimeSetUpAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Test]
    [Description("This test checks if the book is deleted successfully")]
    public async Task DeleteBookAsync_ReturnOK()
    {
        var book = DataHelper.CreateBook();
            
        var httpResponseMessage = 
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);
        
        var deleteResponseMessage = 
            await _libraryHttpService
                .DeleteBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse.Title,
                    bookFromResponse.Author);
        
        Assert.That(deleteResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}