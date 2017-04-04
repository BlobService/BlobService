## Welcome to BlobService

[![Build status](https://ci.appveyor.com/api/projects/status/83uh2apqs8xh92o1?svg=true)](https://ci.appveyor.com/project/Aram/blobservice)
[![Documentation Status](https://readthedocs.org/projects/blobservice/badge/?version=latest)](http://blobservice.readthedocs.io/en/latest/?badge=latest)

This repository contains BlobService for hosting on-permise. 
This is designed as plugable component and can be extended to store blobs in any storage.

The project is now in active development.

Full Documentation is available here.

[http://blobservice.readthedocs.io](http://blobservice.readthedocs.io)


Sample Usage/Configuration.

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var builder = services.AddBlobService(opts =>
        {
            opts.MaxBlobSizeInMB = 100;
        })
        .AddEfMetaStores(opts =>
        {
            opts.ConnectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password = myPassword;"
        })
        .AddFileSystemStorageService(opts =>
        {
            opts.RootPath = @"C:\blobs";
        });
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
    {
        loggerFactory.AddConsole();

        app.UseBlobService();
    }
}
```
