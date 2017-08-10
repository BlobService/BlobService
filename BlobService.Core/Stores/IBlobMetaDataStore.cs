using BlobService.Core.Entities;
using BlobService.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    /// <summary>
    /// Blob MetaData Persisted Store Interface
    /// </summary>
    public interface IBlobMetaDataStore
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="blobId">The blob identifier.</param>
        /// <returns></returns>
        Task<IEnumerable<IBlobMetaData>> GetAllFromBlobAsync(string blobId);

        /// <summary>
        /// Get.
        /// </summary>
        /// <param name="blobId">Blob id</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<string> GetValueAsync(string blobId, string key);


        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="blobId">Blob id</param>
        /// <param name="blobMetaDataModel">MetaData model</param>
        /// <returns></returns>
        Task<IBlobMetaData> AddAsync(string blobId, BlobMetaDataCreateModel blobMetaDataModel);

        /// <summary>
        /// Update.
        /// </summary>
        /// <param name="blobId">Blob id</param>
        /// <param name="key">MetaData key</param>
        /// <param name="value">MetaData value</param>
        /// <returns></returns>
        Task<IBlobMetaData> UpdateAsync(string blobId, string key, string value);

        /// <summary>
        /// Delete Async.
        /// </summary>
        /// <param name="blobId">Blob id</param>
        /// <param name="key">MetaData key</param>
        /// <returns></returns>
        Task DeleteByKeyAsync(string blobId, string key);
    }
}
