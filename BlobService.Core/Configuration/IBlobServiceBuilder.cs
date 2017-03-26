using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Configuration
{
    public interface IBlobServiceBuilder
    {
        IServiceCollection Services { get; }
    }
}
