using System.Net;
using LibraryV4.NUnit.Tests.Api.Fixtures;
using LibraryV4.NUnit.Tests.Api.Services;
using static LibraryV4.NUnit.Tests.Api.TestHelpers.DataHelper;

namespace LibraryV4.NUnit.Tests.Api.Tests;

public class DeleteBookTests : LibraryV4TestFixture
{
    [Test]
    public async Task DeleteBook_ShouldReturnOK()
    {
        //Arrange
        var book = CreateBook();
        await LibraryHttpService.PostBook(book);

        //Act
        var response = await LibraryHttpService.DeleteBook(book.Title, book.Author);

        //Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }

    [Test]
    public async Task DeleteBook_NotExistingBook_ShouldReturnNotFound()
    {
        //Arrange - N/A

        //Act
        var httpResponseMessage = await LibraryHttpService.DeleteBook(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        //Assert
        Assert.That(httpResponseMessage.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
    }
}