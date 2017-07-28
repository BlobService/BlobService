using System.ComponentModel.DataAnnotations;

namespace BlobService.Core.Models
{
    public class ContainerCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
