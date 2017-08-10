using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace BlobService.Core.Models
{
    public class AddBlobViewModel
    {
        public List<BlobMetaDataCreateModel> MetaData { get; set; }
        public IFormFile file { get; set; }
    }
}
