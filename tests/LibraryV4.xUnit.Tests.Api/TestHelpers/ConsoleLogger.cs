using Serilog;

//TODO fix namespace
namespace LibraryV4.xUnit.Tests.Api.Tests.TestHelpers
{
    public static class LoggerHelper
    {
        private static readonly Serilog.ILogger _logger;

        static LoggerHelper()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        public static Serilog.ILogger Logger => _logger;

    }
}
