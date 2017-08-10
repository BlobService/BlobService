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
            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null)
            {
                return NotFound();
            }

            var metaData = await _blobMetaDataStore.GetAllFromBlobAsync(id);
            var metaDataModel = ModelMapper.ToViewModelList(metaData);
            return Ok(metaDataModel);
        }

        [HttpPost("/blobs/{id}/metadata")]
        public async Task<IActionResult> AddMetaDataAsync(string id, [FromBody]BlobMetaDataCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null)
            {
                return NotFound();
            }

            var metaData = await _blobMetaDataStore.AddAsync(id, model);

            return Ok(metaData);
        }
    }
}