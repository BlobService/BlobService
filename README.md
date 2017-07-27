## Welcome to BlobService ## DEVELOP BRANCH

[![Build status](https://ci.appveyor.com/api/projects/status/83uh2apqs8xh92o1?svg=true)](https://ci.appveyor.com/project/Aram/blobservice)
[![Documentation Status](https://readthedocs.org/projects/blobservice/badge/?version=latest)](http://blobservice.readthedocs.io/en/latest/?badge=latest)

This repository contains BlobService for hosting on-permise. 
This is designed as plugable component and can be extended to store blobs in any storage.

The project is now in active development.

Full Documentation is available here.

[http://blobservice.readthedocs.io](http://blobservice.readthedocs.io)


Sample Usage/Configuration.

Install following packages:

`Install-Package BlobService.Core`

`Install-Package BlobService.MetaStore.EntityFrameworkCore`

`Install-Package BlobService.Storage.FileSystem`

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // Adds Blob services core services
        services.AddBlobService(opts =>
        {
            opts.MaxBlobSizeInMB = 100;
        })
        
        // Registers EntityFramework stores for persisting blobs,containers metadata
        .AddEfMetaStores<BlobServiceContext>()
        
        // Registers FileSystem Storage Service for persisting files in filesystem in specified path
        .AddFileSystemStorageService(opts =>
        {
            opts.RootPath = @"C:\blobs";
        });
    }

    public void Configure(IApplicationBuilder app)
    {
        // Use BlobService Middlwares (mvc)
        app.UseBlobService();
    }
}
```
