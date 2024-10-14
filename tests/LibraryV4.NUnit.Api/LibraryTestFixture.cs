using LibraryV4.Services;
using NUnit.Framework;

namespace LibraryV4.Fixtures;

public class LibraryTestFixture
{
    protected readonly LibraryHttpService _libraryHttpService = new();


    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await _libraryHttpService.CreateDefaultUser();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}