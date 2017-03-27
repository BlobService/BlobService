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
        protected readonly IContainerMetaStore _containerStore;

        public ContainersController(ILogger<ContainersController> logger,
            IMapper mapper,
            IContainerMetaStore containerStore)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
        }

        [HttpGet]
        [Route("get")]
        public async Task<IEnumerable<ContainerModel>> Get()
        {
            var containers = await _containerStore.GetAllAsync();
            var containerModels = _mapper.Map<IEnumerable<ContainerMeta>, IEnumerable<ContainerModel>>(containers);
            return containerModels;
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var container = await _containerStore.GetAsync(id);
            if (container == null)
            {
                return NotFound();
            }

            var containerModel = _mapper.Map<ContainerModel>(container);

            return Ok(containerModel);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Create([FromBody]ContainerModel value)
        {
            var container = _mapper.Map<ContainerMeta>(value);
            await _containerStore.AddAsync(container);
            return Ok();
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var container = _containerStore.GetAsync(id);
            if (container != null)
            {
                await _containerStore.RemoveAsync(id);
            }

            return Ok();
        }


        [HttpGet]
        [Route("{id}/blobs")]
        public async Task<IActionResult> Blobs(string id)
        {
            var blobs = await _containerStore.GetBlobsAsync(id);

            if(blobs == null)
            {
                return NotFound();
            }

            var blobsModel = _mapper.Map<IEnumerable<BlobMeta>,IEnumerable<BlobModel>>(blobs);

            return Ok(blobsModel);
        }
    }
}
