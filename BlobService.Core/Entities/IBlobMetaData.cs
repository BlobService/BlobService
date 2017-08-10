namespace BlobService.Core.Entities
{
    public interface IBlobMetaData
    {
        string Id { get; set; }
        string BlobId { get; set; }
        string Key { get; set; }
        string Value { get; set; }
    }
}