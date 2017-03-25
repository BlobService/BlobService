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
            app.UseMvc();
            return app;
        }
    }
}
