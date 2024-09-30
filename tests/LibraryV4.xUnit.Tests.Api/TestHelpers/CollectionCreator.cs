using LibraryV4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryV4.xUnit.Tests.Api.TestHelpers
{
    [CollectionDefinition("Non-Parallel Collection")]
    public class NonParallelCollection : ICollectionFixture<LibraryHttpService>
    {
        // This class has no code, and is never created. Its purpose is simply
    }
}
