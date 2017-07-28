using Microsoft.Extensions.Logging;

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
