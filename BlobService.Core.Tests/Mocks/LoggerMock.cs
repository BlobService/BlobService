using Microsoft.Extensions.Logging;
using System;

namespace BlobService.Core.Tests.Mocks
{
    internal class LoggerMock : ILogger
    {
        public IDisposable BeginScope<TState>(TState state)
        {
            return new LogicalOperationMock();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

        }
    }
}
