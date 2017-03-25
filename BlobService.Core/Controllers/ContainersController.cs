using BlobService.Core.Entities;
using BlobService.Core.Services;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class ContainersController : Controller
    {
        protected IPersistedContainerStore _containerStore;

        public ContainersController(IPersistedContainerStore containerStore)
        {
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
        }

        [HttpGet]
        [Route("get")]
        public async Task<IEnumerable<Container>> Get()
        {
            return await _containerStore.GetAllAsync();
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
            return Ok(container);
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Create([FromBody]Container value)
        {
            await _containerStore.AddAsync(value);
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

            return Ok(blobs);
        }
    }
}
