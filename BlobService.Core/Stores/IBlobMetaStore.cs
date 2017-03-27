using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
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
        Task<BlobMeta> GetAllAsync(string containerId);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<BlobMeta> GetAsync(string key);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task<BlobMeta> AddAsync(BlobMeta blob);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="blob">The BLOB.</param>
        /// <returns></returns>
        Task UpdateAsync(string key, BlobMeta blob);

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task RemoveAsync(string id);
    }
}
