using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using BlobService.Core.Tests.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Core.Tests.Mocks
{
    public class BlobMetaDataStoreMock : IBlobMetaDataStore
    {
        public async Task<IBlobMetaData> AddAsync(string blobId, BlobMetaDataCreateModel blobMetaDataModel)
        {
            return await Task.FromResult(new BlobMetaDataTest()
            {
                Id = "New_Id",
                Key = "Key",
                Value = "Value"
            });
        }

        public async Task DeleteByKeyAsync(string blobId, string key)
        {
            await Task.FromResult(0);
            //return await Task.FromResult(TestData.BlobMetaDataSeed.Where(x => x.BlobId == blobId && x.Key == key));
        }

        public async Task<IEnumerable<IBlobMetaData>> GetAllFromBlobAsync(string blobId)
        {
            return await Task.FromResult(TestData.BlobMetaDataSeed.Where(x => x.BlobId == blobId));
        }

        public Task<string> GetValueAsync(string blobId, string key)
        {
            var bmd = TestData.BlobMetaDataSeed.Where(x => x.BlobId == blobId && x.Key == key).FirstOrDefault();
            return Task.FromResult(bmd.Value);
        }

        public Task<IBlobMetaData> UpdateAsync(string blobId, string key, string value)
        {
            IBlobMetaData bmd = TestData.BlobMetaDataSeed.Where(x => x.BlobId == blobId && x.Key == key).FirstOrDefault();
            bmd.Value = value;
            return Task.FromResult(bmd);
        }
    }
}