using System.Net;
using LibraryV4.Contracts.Domain;
using LibraryV4.Fixtures;
using LibraryV4.TestHelpers;
using Newtonsoft.Json;

namespace LibraryV4.xUnit.Tests.Api;

public class DeleteBookTests : LibraryTestFixture
{
    public DeleteBookTests()
    {
        OneTimeSetUpAsync().GetAwaiter().GetResult();
    }
    
    private async Task OneTimeSetUpAsync()
    {
        await _libraryHttpService.LogIn(_libraryHttpService.DefaultUser, true);
    }

    [Fact]
    public async Task DeleteBookAsync_ReturnOK()
    {
        // Arrange
        var book = DataHelper.CreateBook();
        
        var httpResponseMessage = 
            await _libraryHttpService.PostBook(_libraryHttpService.DefaultUserAuthToken.Token, book);
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        var bookFromResponse = JsonConvert.DeserializeObject<Book>(content);
        
        // Act
        var deleteResponseMessage = 
            await _libraryHttpService
                .DeleteBook(_libraryHttpService.DefaultUserAuthToken.Token, bookFromResponse.Title,
                    bookFromResponse.Author);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, deleteResponseMessage.StatusCode);
    }
}