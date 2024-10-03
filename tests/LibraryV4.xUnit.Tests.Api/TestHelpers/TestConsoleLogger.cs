using Xunit.Abstractions;

//TODO fix namespace
namespace LibraryV4.xUnit.Tests.Api.Tests.TestHelpers
{
    public class TestLoggerHelper
    {
        private readonly ITestOutputHelper _output;

        public TestLoggerHelper(ITestOutputHelper output)
        {
            _output = output;
        }

        public void LogInformation(string message)
        {
            _output.WriteLine(message);
        }
    }
}
