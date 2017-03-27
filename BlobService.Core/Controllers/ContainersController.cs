using AutoMapper;
using BlobService.Core.Entities;
using BlobService.Core.Models;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class ContainersController : Controller
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly IContainerMetaStore _containerMetaStore;

        public ContainersController(
            ILogger<ContainersController> logger,
            IMapper mapper,
            IContainerMetaStore containerMetaStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _containerMetaStore = containerMetaStore ?? throw new ArgumentNullException(nameof(containerMetaStore));
        }

        [HttpGet("/containers")]
        public async Task<IEnumerable<ContainerModel>> Get()
        {
            var containersMetas = await _containerMetaStore.GetAllAsync();

            var containerModels = _mapper.Map<IEnumerable<ContainerMeta>, IEnumerable<ContainerModel>>(containersMetas);

            return containerModels;
        }

        [HttpGet("/containers/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var containerMeta = await _containerMetaStore.GetAsync(id);

            if (containerMeta == null)
            {
                return NotFound();
            }

            var containerModel = _mapper.Map<ContainerModel>(containerMeta);

            return Ok(containerModel);
        }

        [HttpPost("/containers")]
        public async Task<IActionResult> Create([FromBody]ContainerModel value)
        {
            var containerMeta = _mapper.Map<ContainerMeta>(value);

            await _containerMetaStore.AddAsync(containerMeta);

            return Ok();
        }

        [HttpPut("/containers/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody]ContainerModel value)
        {
            var containerMeta = _mapper.Map<ContainerMeta>(value);

            await _containerMetaStore.UpdateAsync(id, containerMeta);

            return Ok();
        }

        [HttpDelete("/containers/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var containerMeta = _containerMetaStore.GetAsync(id);

            if (containerMeta != null)
            {
                await _containerMetaStore.RemoveAsync(id);
            }

            return Ok();
        }


        [HttpGet("/containers/{id}/blobs")]
        public async Task<IActionResult> ListBlobs(string id)
        {
            var blobsMetas = await _containerMetaStore.GetBlobsAsync(id);

            if (blobsMetas == null)
            {
                return NotFound();
            }

            var blobsModel = _mapper.Map<IEnumerable<BlobMeta>, IEnumerable<BlobModel>>(blobsMetas);

            return Ok(blobsModel);
        }
    }
}
