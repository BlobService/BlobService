using BlobService.Core.Services;
using System.Threading.Tasks;

namespace BlobService.Core.Tests.Mocks
{
    internal class StorageServiceMock : IStorageService
    {
        public async Task<string> AddBlobAsync(string containerId, byte[] blob)
        {
            return await Task.FromResult(TestData.Sotragesubject);
        }

        public async Task DeleteBlobAsync(string containerId, string subject)
        {
            await Task.FromResult(0);
        }

        public async Task DeleteContainerAsync(string containerId)
        {
            await Task.FromResult(0);
        }

        public async Task<byte[]> GetBlobAsync(string containerId, string subject)
        {
            if(subject == TestData.Sotragesubject)
            {
                return await Task.FromResult(TestData.FileSeed);
            } else
            {
                return null;
            }
        }

        public async Task<string> UpdateBlobAsync(string containerId, string subject, byte[] blob)
        {
            return await Task.FromResult(TestData.Sotragesubject);
        }
    }
}
