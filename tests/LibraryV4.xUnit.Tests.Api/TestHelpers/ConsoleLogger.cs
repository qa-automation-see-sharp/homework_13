using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
