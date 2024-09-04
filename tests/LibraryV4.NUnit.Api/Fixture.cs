using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryV4.NUnit.Api;

[TestFixture]
public class Fixture
{
    protected LibraryHttpService HttpService;
    protected WebApplicationFactory<IApiMarker> _factory = new();

    [OneTimeSetUp]
    public async Task SetUp()
    {
        var client = _factory.CreateClient();
        HttpService = new LibraryHttpService(client);
        await HttpService.CreateDefaultUser();
        await HttpService.Authorize();
    }

    [SetUp]
    public void SetUpBeforeTest()
    {
        Console.WriteLine("Test starts: " + TestContext.CurrentContext.Test.Name);
    }

    [TearDown]
    public void TearDownAfterTest()
    {
        Console.WriteLine("Test finished: " + TestContext.CurrentContext.Result.Outcome.Status);
        Console.WriteLine("====================================");
    }

    [OneTimeTearDown]
    public void TearDown()
    {
    }
}
