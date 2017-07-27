using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core
{
    public class BlobServiceOptions
    {
        public int MaxBlobSizeInMB { get; set; } = Constants.DefaultBlobSizeLimitInMB;
        public bool RequireAuthenticatedUser { get; set; } = true;

        public void TryValidate()
        {
            if (MaxBlobSizeInMB <= 0)
            {
                throw new InvalidOperationException($"{nameof(MaxBlobSizeInMB)} must be great than zero.");
            }
        }
    }
}
