using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using BlobService.Core.Tests.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Core.Tests.Mocks
{
    internal class BlobStoreMock : IBlobStore
    {
        public async Task<IBlob> AddAsync(BlobCreateModel blobModel)
        {
            return await Task.FromResult(new BlobTest()
            {
                Id = "New_Id",
                ContainerId = blobModel.ContainerId,
                MimeType = blobModel.MimeType,
                OrigFileName = blobModel.OrigFileName,
                SizeInBytes = blobModel.SizeInBytes,
                StorageSubject = blobModel.StorageSubject
            });
        }

        public async Task<IEnumerable<IBlob>> GetAllAsync(string containerId)
        {
            return await Task.FromResult(TestData.BlobSeed.Where(x => x.ContainerId == containerId));
        }

        public async Task<IBlob> GetByIdAsync(string key)
        {
            return await Task.FromResult(TestData.BlobSeed.Where(x => x.Id == key).FirstOrDefault());
        }

        public async Task RemoveAsync(string key)
        {
            await Task.FromResult(0);
        }

        public async Task<IBlob> UpdateAsync(string key, IBlob blob)
        {
            return await Task.FromResult(blob);
        }
    }
}
