using BlobService.Core.Entities;

namespace BlobService.Core.Tests.Entities
{
    class BlobMetaDataTest : IBlobMetaData
    {
        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string BlobId { get; set; }
    }
}
