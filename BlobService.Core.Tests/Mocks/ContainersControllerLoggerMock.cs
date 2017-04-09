using BlobService.Core.Controllers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Tests.Mocks
{
    internal class ContainersControllerLoggerMock : ILogger<ContainersController>
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
