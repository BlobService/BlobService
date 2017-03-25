using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Services
{
    public interface IStorageService
    {
        Task<byte[]> GetBlobAsync(string containerId, string subject);
        Task<string> AddBlobAsync(string containerId, byte[] blob);
        Task DeleteBlobAsync(string containerId, string subject);
    }
}
