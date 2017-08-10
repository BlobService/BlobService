using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using BlobService.Core.Tests.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Core.Tests.Mocks
{
    public class ContainerStoreMock : IContainerStore
    {
        public async Task<IContainer> AddAsync(ContainerCreateModel containerModel)
        {
            return await Task.FromResult(new ContainerTest());
        }

        public async Task<IEnumerable<IContainer>> GetAllAsync()
        {
            return await Task.FromResult(TestData.ContainerSeed);
        }

        public async Task<IContainer> GetAsync(string key)
        {
            return await Task.FromResult(TestData.ContainerSeed.Where(x => x.Id == key).FirstOrDefault());
        }

        public async Task<IEnumerable<IBlob>> GetBlobsAsync(string containerKey)
        {
            return await Task.FromResult(TestData.ContainerSeed.Where(x => x.Id == containerKey).FirstOrDefault()?.Blobs);
        }

        public async Task<IContainer> GetByNameAsync(string name)
        {
            return await Task.FromResult(TestData.ContainerSeed.Where(x => x.Name == name).FirstOrDefault());
        }

        public Task RemoveAsync(string key)
        {
            return Task.FromResult(0);
        }

        public async Task<IContainer> UpdateAsync(string key, IContainer container)
        {
            return await Task.FromResult(new ContainerTest());
        }
    }
}
