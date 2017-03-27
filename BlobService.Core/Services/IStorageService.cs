using System.Threading.Tasks;

namespace BlobService.Core.Services
{
    /// <summary>
    /// Storage Service Interface
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Gets the BLOB asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="subject">The subject.</param>
        /// <returns>Blob's byte array.</returns>
        Task<byte[]> GetBlobAsync(string containerId, string subject);

        /// <summary>
        /// Adds the BLOB asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="blob">The BLOB.</param>
        /// <returns>Blob's subject in storage.</returns>
        Task<string> AddBlobAsync(string containerId, byte[] blob);

        /// <summary>
        /// Updates the BLOB asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="blob">The BLOB.</param>
        /// <returns>Blob's subject in storage.</returns>
        Task<string> UpdateBlobAsync(string containerId, string subject, byte[] blob);

        /// <summary>
        /// Deletes the BLOB asynchronous.
        /// </summary>
        /// <param name="containerId">The container identifier.</param>
        /// <param name="subject">The subject.</param>
        /// <returns></returns>
        Task DeleteBlobAsync(string containerId, string subject);
    }
}
