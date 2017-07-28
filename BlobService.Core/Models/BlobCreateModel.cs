using System.ComponentModel.DataAnnotations;

namespace BlobService.Core.Models
{
    public class BlobCreateModel
    {
        [Required]
        public string ContainerId { get; set; }
        [Required]
        public string OrigFileName { get; set; }
        [Required]
        public int SizeInBytes { get; set; }
        [Required]
        public string MimeType { get; set; }
        [Required]
        public string StorageSubject { get; set; }
    }
}
