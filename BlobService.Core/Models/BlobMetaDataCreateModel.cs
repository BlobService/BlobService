using System.ComponentModel.DataAnnotations;

namespace BlobService.Core.Models
{
    public class BlobMetaDataCreateModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Value { get; set; }
    }
}
