## Welcome to BlobService ##

[![Build status](https://ci.appveyor.com/api/projects/status/83uh2apqs8xh92o1?svg=true)](https://ci.appveyor.com/project/Aram/blobservice)
[![Documentation Status](https://readthedocs.org/projects/blobservice/badge/?version=latest)](http://blobservice.readthedocs.io/en/latest/?badge=latest)

This repository contains BlobService for hosting on-permise. 
This is designed as plugable component and can be extended to store blobs in any storage.

The project is now in active development.

Full Documentation is available here.

[http://blobservice.readthedocs.io](http://blobservice.readthedocs.io)


### Configuration.
1) Create new ASP.NET Core project in you solution

2) Install folowing Nuget packages
```
Install-Package BlobService.Core
Install-Package BlobService.MetaStore.EntityFrameworkCore
Install-Package BlobService.Storage.FileSystem
```

3) Add folowing code to your Startup.cs

```c#
//Usings

using BlobService.Core.Configuration;
using BlobService.MetaStore.EntityFrameworkCore;
using BlobService.MetaStore.EntityFrameworkCore.Configuration;
using BlobService.Storage.FileSystem;
using BlobService.Storage.FileSystem.Configuration;
```

```c#
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
    	// Cors Policy
        services.AddCors(opts =>
            {
                opts.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowCredentials();

                });
            });
    
    
    	// Add DB Context
        services.AddDbContext<BlobServiceContext>(opts =>
            {
            	// Example
                opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BS;Trusted_Connection=True;MultipleActiveResultSets=true");
            });
    
        // Adds Blob services core services
        services.AddBlobService(opts =>
        {
        	opts.CorsPolicyName = "AllowAllOrigins";
            opts.MaxBlobSizeInMB = 100;
        })        
        // Registers FileSystem Storage Service for persisting files in filesystem in specified path
        .AddFileSystemStorageService<FileSystemStorageService>(opts =>
        {
            // Make sure to give Write permission to IIS User
            opts.RootPath = @"C:\blobs";
        })
        .AddEfMetaStores<BlobServiceContext>();
    }

    public void Configure(IApplicationBuilder app)
    {
        // Use BlobService Middlwares (mvc)
        app.UseBlobService();
    }
}
```
4) Execute folowing command in Console
```
Add-Migrations
```
and specify name for your migration

5) And finally execute 
```
Update-Database
```

### Usage.

#### Container Endpoints

1) Get all containers
```
Method: GET
Endpoint: www.example.com/containers
Return Data Sample: [{"name" : "ContainerName" , "id" : "6ffa7213-7958-460c-b6f1-70d019b32356"}]
```

2) Add container
```
Method: POST
Endpoint: www.example.com/containers
Data: {"name" : "ContainerName"}
Return Data Sample: {"name" : "ContainerName" , "id" : "6ffa7213-7958-460c-b6f1-70d019b32356"}
```

3) Get container by name
```
Method: GET
Endpoint: www.example.com/containers/{name}
Return Data Sample: {"name" : "ContainerName" , "id" : "6ffa7213-7958-460c-b6f1-70d019b32356"}
```


