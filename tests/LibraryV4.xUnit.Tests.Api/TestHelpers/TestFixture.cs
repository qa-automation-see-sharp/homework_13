using LibraryV4.Services;
using LibraryV4.xUnit.Tests.Api.Tests.TestHelpers;
using Xunit.Abstractions;

namespace LibraryV4.xUnit.Tests.Api.TestHelpers
{
    public class TestFixture : IAsyncLifetime
    {
        public LibraryHttpService libraryHttpService { get; private set; }
        public TestLoggerHelper TestLoggerHelper { get; private set; }

        public TestFixture()
        {
            libraryHttpService = new LibraryHttpService();
        }

        public void InitializaLogger(ITestOutputHelper output)
        {
            TestLoggerHelper = new TestLoggerHelper(output);
        }

        public async Task InitializeAsync()
        {
            await libraryHttpService.CreateTestUser();
            await libraryHttpService.LoginTestUser();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
        //public LibraryHttpService LibraryHttpService => libraryHttpService;

    }
}
