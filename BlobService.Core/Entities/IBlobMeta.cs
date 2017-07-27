namespace BlobService.Core.Entities
{
    public interface IBlobMeta
    {
        string Id { get; set; }
        string ContainerId { get; set; }
        string OrigFileName { get; set; }
        int SizeInBytes { get; set; }
        string MimeType { get; set; }
        string StorageSubject { get; set; }
    }
}
