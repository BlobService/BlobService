using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BlobService.Core.Tests.Mocks
{
    public class FileMock : IFormFile
    {
        private int? _fileSize;
        public FileMock(int? fileSize = null)
        {
            _fileSize = fileSize;
        }

        public string ContentType => string.Empty;

        public string ContentDisposition => string.Empty;

        public IHeaderDictionary Headers => throw new NotImplementedException();

        public long Length => _fileSize ?? TestData.FileSeed.Length;

        public string Name => string.Empty;

        public string FileName => "Test.txt";

        public void CopyTo(Stream target)
        {
            target.Write(TestData.FileSeed, 0, TestData.FileSeed.Length);
        }

        public async Task CopyToAsync(Stream target, CancellationToken cancellationToken = default(CancellationToken))
        {
            target.Write(TestData.FileSeed, 0, TestData.FileSeed.Length);
            await Task.FromResult(0);
        }

        public Stream OpenReadStream()
        {
            return new MemoryStream(TestData.FileSeed);
        }
    }
}
