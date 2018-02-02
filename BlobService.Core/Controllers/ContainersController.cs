using BlobService.Core.Models;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class ContainersController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger _logger;
        protected readonly IContainerStore _containerStore;
        protected readonly IBlobMetaDataStore _blobMetaDataStore;
        protected readonly IStorageService _storageService;

        public ContainersController(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            IBlobMetaDataStore blobMetaDataStore,
            IContainerStore containerStore,
            IStorageService storageService)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<ContainersController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
            _blobMetaDataStore = blobMetaDataStore ?? throw new ArgumentNullException(nameof(blobMetaDataStore));
            _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        }

        [HttpGet("/containers")]
        public async Task<IActionResult> GetAllContainersAsync()
        {
            var containers = await _containerStore.GetAllAsync();

            var containerModels = ModelMapper.ToViewModelList(containers);

            return Ok(containerModels);
        }

        //[HttpGet("/containers/{id}")]
        //public async Task<IActionResult> GetContainerByIdAsync(string id)
        //{
        //    if (string.IsNullOrEmpty(id)) return NotFound();

        //    var containerMeta = await _containerMetaStore.GetAsync(id);

        //    if (containerMeta == null) return NotFound();

        //    var containerModel = ModelMapper.ToViewModel(containerMeta);

        //    return Ok(containerModel);
        //}

        [HttpGet("/containers/{name}")]
        public async Task<IActionResult> GetContainerByNameAsync(string name)
        {
            if (string.IsNullOrEmpty(name)) return NotFound();

            var container = await _containerStore.GetByNameAsync(name);

            if (container == null) return NotFound();

            var containerModel = ModelMapper.ToViewModel(container);

            return Ok(containerModel);
        }

        [HttpPost("/containers")]
        public async Task<IActionResult> AddContainerAsync([FromBody]ContainerCreateModel model)
        {
            if (model == null) return BadRequest();

            var container = await _containerStore.AddAsync(model);

            var containerModel = ModelMapper.ToViewModel(container);

            return Ok(containerModel);
        }

        [HttpPut("/containers/{id}")]
        public async Task<IActionResult> UpdateContainerAsync(string id, [FromBody]ContainerCreateModel model)
        {
            if (model == null) return BadRequest();

            var container = await _containerStore.GetAsync(id);

            if (container == null) return NotFound();

            container.Name = model.Name;

            var updatedContainer = await _containerStore.UpdateAsync(id, container);
            var updatedContainerModel = ModelMapper.ToViewModel(updatedContainer);

            return Ok(updatedContainerModel);
        }

        [HttpDelete("/containers/{id}")]
        public async Task<IActionResult> DeleteContainerAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var container = await _containerStore.GetAsync(id);
            if (container != null)
            {
                await _containerStore.RemoveAsync(id);
                await _storageService.DeleteContainerAsync(id);
            }

            return Ok();
        }


        [HttpGet("/containers/{id}/blobs")]
        public async Task<IActionResult> ListBlobsAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var blobs = await _containerStore.GetBlobsAsync(id);

            if (blobs == null) return NotFound();

            var blobsModel = ModelMapper.ToViewModelList(blobs).ToList();
            return Ok(blobsModel);
        }
    }
}
