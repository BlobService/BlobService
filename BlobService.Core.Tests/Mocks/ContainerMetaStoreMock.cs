using BlobService.Core.Stores;
using System.Collections.Generic;
using BlobService.Core.Entities;
using BlobService.Core.Models;
using System.Threading.Tasks;
using BlobService.Core.Tests.Entities;
using System.Linq;

namespace BlobService.Core.Tests.Mocks
{
    public class ContainerMetaStoreMock : IContainerMetaStore
    {
        public async Task<IContainerMeta> AddAsync(ContainerCreateModel containerModel)
        {
            return await Task.FromResult(new ContainerMetaTest());
        }

        public async Task<IEnumerable<IContainerMeta>> GetAllAsync()
        {
            return await Task.FromResult(TestData.ContainerMetaSeed);
        }

        public async Task<IContainerMeta> GetAsync(string key)
        {
            return await Task.FromResult(TestData.ContainerMetaSeed.Where(x => x.Id == key).FirstOrDefault());
        }

        public async Task<IEnumerable<IBlobMeta>> GetBlobsAsync(string containerKey)
        {
            return await Task.FromResult(TestData.ContainerMetaSeed.Where(x => x.Id == containerKey).FirstOrDefault()?.Blobs);
        }

        public async Task<IContainerMeta> GetByNameAsync(string name)
        {
            return await Task.FromResult(TestData.ContainerMetaSeed.Where(x => x.Name == name).FirstOrDefault());
        }

        public Task RemoveAsync(string key)
        {
            return Task.FromResult(0);
        }

        public async Task<IContainerMeta> UpdateAsync(string key, IContainerMeta container)
        {
            return await Task.FromResult(new ContainerMetaTest());
        }
    }
}
