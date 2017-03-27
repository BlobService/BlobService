using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Helpers
{
    public static class IFormFileExtensions
    {
        public static async Task<byte[]> ToByteArrayAsync(this IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using (var fileStream = file.OpenReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await fileStream.CopyToAsync(memoryStream);

                    var fileName = file.FileName;
                    var buffer = memoryStream.ToArray();

                    return buffer;
                }
            }
        }
    }
}
