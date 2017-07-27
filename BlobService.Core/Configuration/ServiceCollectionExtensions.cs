using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
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

                if (blobServiceOptions.RequireAuthenticatedUser)
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();
                    options.Filters.Add(new AuthorizeFilter(policy));
                }

            });

            services.AddSingleton(typeof(BlobServiceOptions), blobServiceOptions);

            var builder = new BlobServiceBuilder(services);

            return builder;
        }
    }
}
