using BlobService.Core.Helpers;
using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class BlobsController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger<BlobsController> _logger;
        protected readonly IStorageService _storageService;
        protected readonly IBlobStore _blobStore;
        protected readonly IBlobMetaDataStore _blobMetaDataStore;
        protected readonly IContainerStore _containerStore;

        public BlobsController(
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

        [HttpGet("/blobs/{id}")]
        public async Task<IActionResult> GetBlobByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null) return NotFound();

            var blobMetaData = await _blobMetaDataStore.GetAllFromBlobAsync(blob.Id);

            var blobModel = ModelMapper.ToViewModel(blob);
            blobModel.MetaData = ModelMapper.ToViewModelList(blobMetaData);

            return Ok(blobModel);
        }

        [HttpGet("/blobs/{id}/raw")]
        public async Task<IActionResult> RawBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null) return NotFound();

            var blobContent = await _storageService.GetBlobAsync(blob.ContainerId, blob.StorageSubject);

            if (blobContent == null) return NotFound();

            return File(blobContent, blob.MimeType, blob.OrigFileName);
        }

        [HttpPost("/containers/{containerId}/blobs")]
        // TODO add uploading by chunks
        // TODO refactor 
        public async Task<IActionResult> AddBlobAsync(string containerId, List<BlobMetaDataCreateModel> MetaData, IFormFile file)
        {
            if (file == null) return BadRequest();

            var container = await _containerStore.GetAsync(containerId);

            if (container == null) return NotFound();

            if (FileLengthExceedAllowed(file)) return BadRequest();

            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.AddBlobAsync(containerId, buffer);

            if (string.IsNullOrEmpty(subject)) return StatusCode(500);

            string mimeType = MimeMapping.GetMimeMapping(fileName);

            var blob = await _blobStore.AddAsync(new BlobCreateModel()
            {
                ContainerId = containerId,
                OrigFileName = fileName,
                MimeType = mimeType,
                StorageSubject = subject,
                SizeInBytes = buffer.Length
            });

            foreach (var singleMetaData in MetaData)
            {
                await _blobMetaDataStore.AddAsync(blob.Id, singleMetaData);
            }

            var blobModel = ModelMapper.ToViewModel(blob);
            blobModel.MetaData = ModelMapper.ToViewModelList(await _blobMetaDataStore.GetAllFromBlobAsync(blob.Id));
            return Ok(blobModel);
        }


        [HttpPut("/blobs/{id}")]
        // TODO add uploading by chunks
        // TODO refactor 
        public async Task<IActionResult> UpdateBlobAsync(string id, IFormFile file)
        {
            if (file == null) return BadRequest();

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null) return NotFound();

            if (FileLengthExceedAllowed(file)) return BadRequest();

            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.UpdateBlobAsync(blob.ContainerId, blob.StorageSubject, buffer);

            if (string.IsNullOrEmpty(subject)) return StatusCode(500);

            blob.MimeType = MimeMapping.GetMimeMapping(fileName);
            blob.SizeInBytes = buffer.Length;
            blob.StorageSubject = subject;
            blob.OrigFileName = fileName;

            await _blobStore.UpdateAsync(blob.Id, blob);

            var blobModel = ModelMapper.ToViewModel(blob);

            return Ok(blobModel);
        }

        [HttpDelete("blobs/{id}")]
        public async Task<IActionResult> DeleteBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blob = await _blobStore.GetByIdAsync(id);

            if (blob == null) return NotFound();

            await _blobStore.RemoveAsync(id);

            await _storageService.DeleteBlobAsync(blob.ContainerId, blob.Id);

            return Ok();
        }

        internal bool FileLengthExceedAllowed(IFormFile file)
        {
            int fileLengthInMb = (int)(file.Length / 1024 / 1024);

            if (fileLengthInMb > _options.MaxBlobSizeInMB)
                return true;

            return false;
        }
    }
}
