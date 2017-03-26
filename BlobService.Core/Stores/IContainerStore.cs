using BlobService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Stores
{
    /// <summary>
    /// Containers Persited Store Interface
    /// </summary>
    public interface IContainerStore
    {
        /// <summary>
        /// Gets all containers asynchronous.
        /// </summary>
        /// <returns>Containers</returns>
        Task<IEnumerable<Container>> GetAllAsync();

        /// <summary>
        /// Gets the container by key asynchronous.
        /// </summary>
        /// <param name="key">The container key.</param>
        /// <returns>Container</returns>
        Task<Container> GetAsync(string key);

        /// <summary>
        /// Gets the container by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Container</returns>
        Task<Container> GetByNameAsync(string name);

        /// <summary>
        /// Gets the blobs by container key asynchronous.
        /// </summary>
        /// <param name="containerKey">The container key.</param>
        /// <returns>Blobs of container</returns>
        Task<IEnumerable<Blob>> GetBlobsAsync(string containerKey);

        /// <summary>
        /// Adds the container asynchronous.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        Task AddAsync(Container container);

        /// <summary>
        /// Updates the container asynchronous.
        /// </summary>
        /// <param name="key">The container key to update.</param>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        Task UpdateAsync(string key, Container container);

        /// <summary>
        /// Removes the container asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveAsync(string key);
    }
}
