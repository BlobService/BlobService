using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Models
{
    public class BlobModel
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string DownloadUrl { get; set; }
    }
}
