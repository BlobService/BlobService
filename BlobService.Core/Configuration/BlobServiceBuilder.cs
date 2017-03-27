using System;
using Microsoft.Extensions.DependencyInjection;

namespace BlobService.Core.Configuration
{
    public class BlobServiceBuilder : IBlobServiceBuilder
    {
        public BlobServiceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IServiceCollection Services { get; }
    }
}
