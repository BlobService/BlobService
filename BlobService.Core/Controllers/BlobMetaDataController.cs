using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class BlobMetaDataController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger<BlobsController> _logger;
        protected readonly IStorageService _storageService;
        protected readonly IBlobStore _blobStore;
        protected readonly IBlobMetaDataStore _blobMetaDataStore;
        protected readonly IContainerStore _containerStore;

        public BlobMetaDataController(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            IStorageService storageService,
            IBlobStore blobStore,
            IBlobMetaDataStore blobMetaDataStore,
            IContainerStore containerStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<BlobsController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _blobStore = blobStore ?? throw new ArgumentNullException(nameof(blobStore));
            _blobMetaDataStore = blobMetaDataStore ?? throw new ArgumentNullException(nameof(blobMetaDataStore));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
        }

        [HttpGet("/blobs/{id}/metadata")]
        public async Task<IActionResult> GetMetaDataAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null)
            {
                return NotFound();
            }

            var metaData = await _blobMetaDataStore.GetAllFromBlobAsync(id);
            var metaDataModel = ModelMapper.ToViewModelList(metaData);
            return Ok(metaDataModel);
        }

        [HttpDelete("/blobs/{blobId}/metadata")]
        public async Task<IActionResult> DeleteMetaDataAsync(string blobId, string key)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(blobId))
            {
                return BadRequest();
            }

            var blob = await _blobStore.GetByIdAsync(blobId);

            if (blob == null)
            {
                return NotFound();
            }

            await _blobMetaDataStore.DeleteByKeyAsync(blobId, key);
            return Ok();
        }

        [HttpPost("/blobs/{blobId}/metadata")]
        public async Task<IActionResult> SetMetaDataAsync(string blobId, [FromBody]BlobMetaDataCreateModel model)
        {
            if (string.IsNullOrEmpty(blobId) || model == null)
            {
                return BadRequest();
            }

            var blob = await _blobStore.GetByIdAsync(blobId);

            if (blob == null)
            {
                return NotFound();
            }

            var metaData = await _blobMetaDataStore.AddAsync(blobId, model);

            return Ok(metaData);
        }

        [HttpPut("/blobs/{blobId}/metadata")]
        public async Task<IActionResult> UpdateMetaDataAsync(string blobId, [FromBody]BlobMetaDataCreateModel model)
        {
            if (string.IsNullOrEmpty(blobId) || model == null)
            {
                return BadRequest();
            }

            var blob = await _blobStore.GetByIdAsync(blobId);

            if (blob == null)
            {
                return NotFound();
            }

            var metaData = await _blobMetaDataStore.UpdateAsync(blobId, model.Key, model.Value);

            return Ok(metaData);
        }
    }
}