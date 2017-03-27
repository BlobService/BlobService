﻿using AutoMapper;
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
    public class BlobsController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly IStorageService _storageService;
        protected readonly IBlobMetaStore _blobStore;
        protected readonly IContainerMetaStore _containerStore;

        public BlobsController(ILogger<BlobsController> logger,
            IMapper mapper,
            IStorageService storageService,
            IBlobMetaStore blobStore,
            IContainerMetaStore containerStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
            _blobStore = blobStore ?? throw new ArgumentNullException(nameof(blobStore));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }
            var blob = await _blobStore.GetAsync(id);
            if (blob == null)
            {
                return NotFound();
            }

            var blobModel = _mapper.Map<BlobModel>(blob);

            return Ok(blobModel);
        }

        [HttpGet]
        [Route("download/{id}")]
        public async Task<HttpResponseMessage> Download(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            var blob = await _blobStore.GetAsync(id);
            if (blob == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            var blobContent = await _storageService.GetBlobAsync(blob.ContainerId, blob.StorageSubject);

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
                FileName = blob.OrigFileName
            };

            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(blob.MimeType);

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
                var container = await _containerStore.GetAsync(containerId);
                if (container == null)
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
                            var blob = new BlobMeta()
                            {
                                ContainerId = containerId,
                                OrigFileName = fileName,
                                MimeType = mimeType,
                                StorageSubject = subject,
                                SizeInBytes = buffer.Length
                            };

                            blob = await _blobStore.AddAsync(blob);

                            var blobModel = _mapper.Map<BlobModel>(blob);

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
            var blob = await _blobStore.GetAsync(id);
            if (blob == null)
            {
                return NotFound();
            }

            await _blobStore.RemoveAsync(id);

            await _storageService.DeleteBlobAsync(blob.ContainerId, blob.Id);

            return Ok();
        }
    }
}
