using BlobService.Core.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BlobService.Core.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IBlobServiceBuilder AddBlobService(this IServiceCollection services, Action<BlobServiceOptions> setupAction = null)
        {
            var blobServiceOptions = new BlobServiceOptions();

            setupAction?.Invoke(blobServiceOptions);

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(AppExceptionFilterAttribute));
            });

            services.AddSingleton(typeof(BlobServiceOptions), blobServiceOptions);

            var builder = new BlobServiceBuilder(services);

            return builder;
        }
    }
}
