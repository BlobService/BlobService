using System.Collections;
using System.Collections.Generic;

namespace BlobService.Core.Entities
{
    public interface IContainerMeta
    {
        string Id { get; set; }
        string Name { get; set; }
    }
}
