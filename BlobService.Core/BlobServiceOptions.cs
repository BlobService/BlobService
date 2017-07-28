using System;

namespace BlobService.Core
{
    public class BlobServiceOptions
    {
        public int MaxBlobSizeInMB { get; set; } = Constants.DefaultBlobSizeLimitInMB;
        public bool RequireSASAuthorization { get; set; } = true;
        public string CorsPolicyName { get; set; }

        public void TryValidate()
        {
            if (MaxBlobSizeInMB <= 0)
            {
                throw new InvalidOperationException($"{nameof(MaxBlobSizeInMB)} must be great than zero.");
            }

            if (string.IsNullOrEmpty(CorsPolicyName))
            {
                throw new InvalidOperationException($"{nameof(CorsPolicyName)} cant be null.");
            }
        }
    }
}
