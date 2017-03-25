using BlobService.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core
{
    public class BlobServiceOptions
    {
        public IStorageService StorageService { get; set; }
    }
}
