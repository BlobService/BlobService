using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Configuration
{
    public static class BlobServiceApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseBlobService(this IApplicationBuilder app)
        {
            app.Validate();

            app.UseMvc();

            return app;
        }

        private static void Validate(this IApplicationBuilder app)
        {
            var storageService = app.ApplicationServices.GetService(typeof(IStorageService));
            if (storageService == null) throw new InvalidOperationException("StorageService isn't registered.");

            var blobStore = app.ApplicationServices.GetService(typeof(IBlobMetaStore));
            if (blobStore == null) throw new InvalidOperationException("BlobStore isn't registered.");

            var containerStore = app.ApplicationServices.GetService(typeof(IContainerMetaStore));
            if (containerStore == null) throw new InvalidOperationException("ContainerStore isn't registered.");
        }
    }
}
