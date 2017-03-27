using AutoMapper;
using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    [Route("blobs")]
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

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var blobMeta = await _blobMetaStore.GetAsync(id);

            if (blobMeta == null)
            {
                return NotFound();
            }

            var blobModel = _mapper.Map<BlobModel>(blobMeta);

            return Ok(blobModel);
        }

        [HttpGet]
        [Route("download/{id}")]
        public async Task<HttpResponseMessage> Download(string id)
        {
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

        [HttpPost]
        [Route("add")]
        // TODO #1 add uploading by chanks
        // TODO #2 get rid of Request to make this function testable
        public async Task<IActionResult> Add(string containerId)
        {
            if (Request.Form.Files != null && Request.Form.Files.Count > 0)
            {
                var containerMeta = await _containerMetaStore.GetAsync(containerId);

                if (containerMeta == null)
                {
                    return NotFound();
                }

                var file = Request.Form.Files[0];
                var contentType = file.ContentType;

                using (var fileStream = file.OpenReadStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await fileStream.CopyToAsync(memoryStream);

                        var fileName = file.FileName;
                        var buffer = memoryStream.ToArray();

                        var subject = await _storageService.AddBlobAsync(containerId, buffer);

                        if (!string.IsNullOrEmpty(subject))
                        {
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
                    }
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
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
