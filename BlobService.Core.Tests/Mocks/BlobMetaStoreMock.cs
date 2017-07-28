using BlobService.Core.Stores;
using System.Collections.Generic;
using BlobService.Core.Entities;
using BlobService.Core.Models;
using System.Threading.Tasks;
using BlobService.Core.Tests.Entities;
using System.Linq;

namespace BlobService.Core.Tests.Mocks
{
    internal class BlobMetaStoreMock : IBlobMetaStore
    {
        public async Task<IBlobMeta> AddAsync(BlobCreateModel blobModel)
        {
            return await Task.FromResult(new BlobMetaTest()
            {
                Id = "New_Id",
                ContainerId = blobModel.ContainerId,
                MimeType = blobModel.MimeType,
                OrigFileName = blobModel.OrigFileName,
                SizeInBytes = blobModel.SizeInBytes,
                StorageSubject = blobModel.StorageSubject
            });
        }

        public async Task<IEnumerable<IBlobMeta>> GetAllAsync(string containerId)
        {
            return await Task.FromResult(TestData.BlobMetaSeed.Where(x => x.ContainerId == containerId));
        }

        public async Task<IBlobMeta> GetAsync(string key)
        {
            return await Task.FromResult(TestData.BlobMetaSeed.Where(x => x.Id == key).FirstOrDefault());
        }

        public async Task RemoveAsync(string key)
        {
            await Task.FromResult(0);
        }

        public async Task<IBlobMeta> UpdateAsync(string key, IBlobMeta blob)
        {
            return await Task.FromResult(blob);
        }
    }
}
