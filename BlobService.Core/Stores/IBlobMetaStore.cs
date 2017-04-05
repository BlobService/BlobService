using BlobService.Core.Entities;
using BlobService.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    /// <summary>
    /// Blobs Persisted Store Interface
    /// </summary>
    public interface IBlobMetaStore<TBlobMeta> 
        where TBlobMeta : IBlobMeta
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<TBlobMeta>> GetAllAsync(string containerId);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<TBlobMeta> GetAsync(string key);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task<TBlobMeta> AddAsync(BlobCreateModel blobModel);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task<TBlobMeta> UpdateAsync(string key, TBlobMeta blob);

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
