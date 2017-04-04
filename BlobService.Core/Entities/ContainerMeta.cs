using System.Collections;
using System.Collections.Generic;

namespace BlobService.Core.Entities
{
    public class ContainerMeta
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<BlobMeta> Blobs { get; set; } = new HashSet<BlobMeta>();
    }
}
