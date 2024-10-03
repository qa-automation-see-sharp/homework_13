using LibraryV4.Services;

namespace LibraryV4.xUnit.Tests.Api.TestHelpers
{
    [CollectionDefinition("Non-Parallel Collection")]
    public class NonParallelCollection : ICollectionFixture<LibraryHttpService>
    {
        // This class has no code, and is never created. Its purpose is simply
    }
}
