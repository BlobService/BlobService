using BlobService.Core.Entities;
using System.Collections.Generic;

namespace BlobService.Core.Tests.Entities
{
    public class ContainerMetaTest : IContainerMeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<IBlobMeta> Blobs { get; set; } = new HashSet<IBlobMeta>();
    }
}
