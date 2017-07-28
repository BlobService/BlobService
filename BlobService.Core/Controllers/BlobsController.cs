using BlobService.Core.Helpers;
using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class BlobsController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger<BlobsController> _logger;
        protected readonly IStorageService _storageService;
        protected readonly IBlobMetaStore _blobMetaStore;
        protected readonly IContainerMetaStore _containerMetaStore;

        public BlobsController(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            IStorageService storageService,
            IBlobMetaStore blobMetaStore,
            IContainerMetaStore containerMetaStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<BlobsController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _blobMetaStore = blobMetaStore ?? throw new ArgumentNullException(nameof(blobMetaStore));
            _containerMetaStore = containerMetaStore ?? throw new ArgumentNullException(nameof(containerMetaStore));
        }

        [HttpGet("/blobs/{id}")]
        public async Task<IActionResult> GetBlobByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null) return NotFound();

            var blobModel = ModelMapper.ToViewModel(blobMeta);

            return Ok(blobModel);
        }

        [HttpGet("/blobs/{id}/raw")]
        public async Task<IActionResult> RawBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null) return NotFound();

            var blobContent = await _storageService.GetBlobAsync(blobMeta.ContainerId, blobMeta.StorageSubject);

            if (blobContent == null) return NotFound();

            return File(blobContent, blobMeta.MimeType, blobMeta.OrigFileName);
        }

        [HttpPost("/containers/{containerId}/blobs")]
        // TODO add uploading by chunks
        // TODO refactor 
        public async Task<IActionResult> AddBlobAsync(string containerId, IFormFile file)
        {
            if (file == null) return BadRequest();

            var containerMeta = await _containerMetaStore.GetAsync(containerId);

            if (containerMeta == null) return NotFound();

            if (FileLengthExceedAllowed(file)) return BadRequest();
            
            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.AddBlobAsync(containerId, buffer);

            if (string.IsNullOrEmpty(subject)) return StatusCode(500);

            string mimeType = MimeMapping.GetMimeMapping(fileName);

            var blobMeta = await _blobMetaStore.AddAsync(new BlobCreateModel()
            {
                ContainerId = containerId,
                OrigFileName = fileName,
                MimeType = mimeType,
                StorageSubject = subject,
                SizeInBytes = buffer.Length
            });

            var blobModel = ModelMapper.ToViewModel(blobMeta);

            return Ok(blobModel);
        }


        [HttpPut("/blobs/{id}")]
        // TODO add uploading by chunks
        // TODO refactor 
        public async Task<IActionResult> UpdateBlobAsync(string id, IFormFile file)
        {
            if (file == null) return BadRequest();

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null) return NotFound();

            if (FileLengthExceedAllowed(file)) return BadRequest();

            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.UpdateBlobAsync(blobMeta.ContainerId, blobMeta.StorageSubject, buffer);

            if (string.IsNullOrEmpty(subject)) return StatusCode(500);

            blobMeta.MimeType = MimeMapping.GetMimeMapping(fileName);
            blobMeta.SizeInBytes = buffer.Length;
            blobMeta.StorageSubject = subject;
            blobMeta.OrigFileName = fileName;

            await _blobMetaStore.UpdateAsync(blobMeta.Id, blobMeta);

            var blobModel = ModelMapper.ToViewModel(blobMeta);

            return Ok(blobModel);
        }

        [HttpDelete("blobs/{id}")]
        public async Task<IActionResult> DeleteBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null) return NotFound();

            await _blobMetaStore.RemoveAsync(id);

            await _storageService.DeleteBlobAsync(blobMeta.ContainerId, blobMeta.Id);

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
