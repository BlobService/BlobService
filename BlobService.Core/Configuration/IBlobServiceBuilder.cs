using Microsoft.Extensions.DependencyInjection;

namespace BlobService.Core.Configuration
{
    public interface IBlobServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
