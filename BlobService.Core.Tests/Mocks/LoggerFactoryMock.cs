using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Tests.Mocks
{
    class LoggerFactoryMock : ILoggerFactory
    {
        public void AddProvider(ILoggerProvider provider)
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new LoggerMock();
        }

        public void Dispose()
        {
            
        }
    }
}
