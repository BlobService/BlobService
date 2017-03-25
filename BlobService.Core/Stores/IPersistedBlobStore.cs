using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    public interface IPersistedBlobStore
    {
        Task<Blob> GetAllAsync(string containerId);
        Task<Blob> GetAsync(string key);
        Task<Blob> AddAsync(Blob blob);
        Task UpdateAsync(string key, Blob blob);
        Task RemoveAsync(string id);
    }
}
