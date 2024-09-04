using LibraryV4.NUnit.Api.Services;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV4.NUnit.Api.Fixtures;

[TestFixture]
public class LibraryV4TestFixture
{
    protected LibraryHttpService LibraryHttpService;
    protected WebApplicationFactory<IApiMarker> _factory = new(); 

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        var httpClient = _factory.CreateClient();
        LibraryHttpService = new LibraryHttpService(httpClient);
        await LibraryHttpService.CreateDefaultUser();
        await LibraryHttpService.Authorize();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}