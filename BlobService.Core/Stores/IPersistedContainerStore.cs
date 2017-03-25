using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    public interface IPersistedContainerStore
    {
        Task<IEnumerable<Container>> GetAllAsync();
        Task<Container> GetAsync(string key);
        Task<Container> GetByNameAsync(string name);
        Task<IEnumerable<Blob>> GetBlobsAsync(string key);
        Task AddAsync(Container container);
        Task UpdateAsync(string key, Container container);
        Task RemoveAsync(string id);
    }
}
