using BlobService.Core.Entities;
using System.Collections.Generic;

namespace BlobService.Core.Tests.Entities
{
    class BlobTest : IBlob
    {
        public string Id { get; set; }
        public string ContainerId { get; set; }
        public string OrigFileName { get; set; }
        public int SizeInBytes { get; set; }
        public string MimeType { get; set; }
        public string StorageSubject { get; set; }
        public IContainer Container { get; set; }
        public IEnumerable<IBlobMetaData> MetaData { get; }
    }
}
