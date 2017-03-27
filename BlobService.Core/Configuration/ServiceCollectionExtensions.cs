using AutoMapper;
using BlobService.Core.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BlobService.Core.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IBlobServiceBuilder AddBlobService(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(AppExceptionFilterAttribute));
            });
            services.AddAutoMapper(typeof(MappingProfile));

            var builder = new BlobServiceBuilder(services);

            return builder;
        }
    }
}
