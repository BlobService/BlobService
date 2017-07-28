using System;

namespace BlobService.Core.Security
{
    public enum SASTokenSubjects
    {
        Container,
        Blob
    }

    public class SASToken
    {
        public string Id { get; set; }
        public bool IsInfinity { get; set; }
        public DateTimeOffset SignedStart { get; set; }
        public DateTimeOffset SignedExpiry { get; set; }
        public SASTokenSubjects TokenSubject { get; set; }
        public string BlobId { get; set; }
        public BlobAccessTypes BlobAccessType { get; set; }
        public string ContainerId { get; set; }
        public ContainerAccessTypes ContainerAccessType { get; set; }
    }
}
