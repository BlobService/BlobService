using Microsoft.AspNetCore.Mvc.Cors.Internal;
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

            blobServiceOptions.TryValidate();

            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(blobServiceOptions.CorsPolicyName));

            });

            services.AddSingleton(typeof(BlobServiceOptions), blobServiceOptions);

            var builder = new BlobServiceBuilder(services);

            return builder;
        }
    }
}
