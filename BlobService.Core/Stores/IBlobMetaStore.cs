using BlobService.Core.Entities;
using BlobService.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    /// <summary>
    /// Blobs Persisted Store Interface
    /// </summary>
    public interface IBlobMetaStore
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IBlobMeta>> GetAllAsync(string containerId);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<IBlobMeta> GetAsync(string key);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task<IBlobMeta> AddAsync(BlobCreateModel blobModel);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task<IBlobMeta> UpdateAsync(string key, IBlobMeta blob);

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
