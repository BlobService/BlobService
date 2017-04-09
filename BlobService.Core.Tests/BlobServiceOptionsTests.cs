using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace BlobService.Core.Tests
{
    public class BlobServiceOptionsTests
    {
        [Fact]
        public void BlobServiceOptions_DefaultMaxBlobSize_ShouldBeDefaultBlobSizeLimitInMB()
        {
            var options = new BlobServiceOptions();
            Assert.True(options.MaxBlobSizeInMB == Constants.DefaultBlobSizeLimitInMB);
        }

        [Fact]
        public void BlobServiceOptionsValidate_ShouldSuccess()
        {
            var options = new BlobServiceOptions();
            options.TryValidate();
            Assert.True(true);
        }

        [Fact]
        public void BlobServiceOptionsValidate_BlobSizeLimitCantBeLessThanZero_ShouldThrow()
        {
            var options = new BlobServiceOptions();
            options.MaxBlobSizeInMB = -1;
            Assert.Throws<InvalidOperationException>(() => options.TryValidate());
        }
    }
}
