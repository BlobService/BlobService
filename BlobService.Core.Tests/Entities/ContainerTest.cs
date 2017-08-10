using BlobService.Core.Entities;
using System.Collections.Generic;

namespace BlobService.Core.Tests.Entities
{
    public class ContainerTest : IContainer
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<IBlob> Blobs { get; set; } = new HashSet<IBlob>();
    }
}
