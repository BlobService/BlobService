using BlobService.Core.Security;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    public interface ISASStore
    {
        Task<SASToken> GetAsync(string key);
        Task<SASToken> AddAsync(SASToken token);
    }
}
