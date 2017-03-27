using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace BlobService.Core.Configuration
{
    public static class BlobServiceServiceCollectionExtensions
    {
        public static IBlobServiceBuilder AddBlobService(this IServiceCollection services)
        {
            services.AddMvc();
            services.AddAutoMapper(typeof(MappingProfile));
            var builder = new BlobServiceBuilder(services);
            return builder;
        }
    }
}
