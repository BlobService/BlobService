using AutoMapper;
using BlobService.Core.Entities;
using BlobService.Core.Helpers;
using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class BlobsController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly IStorageService _storageService;
        protected readonly IBlobMetaStore _blobMetaStore;
        protected readonly IContainerMetaStore _containerMetaStore;

        public BlobsController(
            ILogger<BlobsController> logger,
            IMapper mapper,
            IStorageService storageService,
            IBlobMetaStore blobMetaStore,
            IContainerMetaStore containerMetaStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _blobMetaStore = blobMetaStore ?? throw new ArgumentNullException(nameof(blobMetaStore));
            _containerMetaStore = containerMetaStore ?? throw new ArgumentNullException(nameof(containerMetaStore));
        }

        [HttpGet("/blobs/{id}")]
        public async Task<IActionResult> GetBlobsById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null)
            {
                return NotFound();
            }

            var blobModel = _mapper.Map<BlobModel>(blobMeta);

            return Ok(blobModel);
        }

        [HttpGet("/blobs/{id}/download")]
        public async Task<HttpResponseMessage> DownloadBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var blobContent = await _storageService.GetBlobAsync(blobMeta.ContainerId, blobMeta.StorageSubject);

            if (blobContent == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(blobContent)
            };

            result.Content.Headers.ContentDisposition =
            new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
            {
                FileName = blobMeta.OrigFileName
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blobMeta.MimeType);

            return result;
        }

        [HttpPost("/containers/{containerId}/blobs")]
        // TODO add uploading by chunks
        public async Task<IActionResult> AddBlobAsync(string containerId, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            var containerMeta = await _containerMetaStore.GetAsync(containerId);

            if (containerMeta == null)
            {
                return NotFound();
            }

            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.AddBlobAsync(containerId, buffer);

            if (string.IsNullOrEmpty(subject))
            {
                return StatusCode(500);
            }

            string mimeType = MimeMapping.GetMimeMapping(fileName);
            var blobMeta = new BlobMeta()
            {
                ContainerId = containerId,
                OrigFileName = fileName,
                MimeType = mimeType,
                StorageSubject = subject,
                SizeInBytes = buffer.Length
            };

            blobMeta = await _blobMetaStore.AddAsync(blobMeta);

            var blobModel = _mapper.Map<BlobModel>(blobMeta);

            return Ok(blobModel);
        }

        [HttpPut("/blobs/{blobId}")]
        // TODO add uploading by chunks
        public async Task<IActionResult> UpdateBlobAsync(string blobId, IFormFile file)
        {
            if (file == null)
            {
                return BadRequest();
            }

            var blobMeta = await _blobMetaStore.GetAsync(blobId);

            if (blobMeta == null)
            {
                return NotFound();
            }

            var contentType = file.ContentType;
            var fileName = file.FileName;
            var buffer = await file.ToByteArrayAsync();

            var subject = await _storageService.UpdateBlobAsync(blobMeta.ContainerId, blobMeta.StorageSubject, buffer);

            if (string.IsNullOrEmpty(subject))
            {
                return StatusCode(500);
            }

            blobMeta.MimeType = MimeMapping.GetMimeMapping(fileName);
            blobMeta.SizeInBytes = buffer.Length;
            blobMeta.StorageSubject = subject;
            blobMeta.OrigFileName = fileName;

            await _blobMetaStore.UpdateAsync(blobMeta.Id, blobMeta);

            var blobModel = _mapper.Map<BlobModel>(blobMeta);

            return Ok(blobModel);
        }

        [HttpDelete("blobs/{id}")]
        public async Task<IActionResult> DeleteBlobAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null)
            {
                return NotFound();
            }

            await _blobMetaStore.RemoveAsync(id);

            await _storageService.DeleteBlobAsync(blobMeta.ContainerId, blobMeta.Id);

            return Ok();
        }
    }
}
