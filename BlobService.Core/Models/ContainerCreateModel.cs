using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BlobService.Core.Models
{
    public class ContainerCreateModel
    {
        [Required]
        public string Name { get; set; }
    }
}
