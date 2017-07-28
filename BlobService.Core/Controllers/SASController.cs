using BlobService.Core.Security;
using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlobService.Core.Controllers
{
    public class SASController : Controller
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger _logger;
        protected readonly ISASStore _sasStore;
        protected readonly IContainerMetaStore _containerMetaStore;
        protected readonly IBlobMetaStore _blobMetaStore;

        public SASController(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            ISASStore sasStore,
            IContainerMetaStore containerMetaStore,
            IBlobMetaStore blobMetaStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<ContainersController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _sasStore = sasStore ?? throw new ArgumentNullException(nameof(sasStore));
            _containerMetaStore = containerMetaStore ?? throw new ArgumentNullException(nameof(containerMetaStore));
            _blobMetaStore = blobMetaStore ?? throw new ArgumentNullException(nameof(blobMetaStore));
        }


        [HttpGet("/sas/{id}")]
        public async Task<IActionResult> GetSAS(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();

            var sas = await _sasStore.GetAsync(id);

            if (sas == null) return NotFound();

            return Ok(sas);
        }

        [HttpPost("/sas")]
        public async Task<IActionResult> CreateSAS(SASToken token)
        {
            if (token == null) return BadRequest();

            switch(token.TokenSubject)
            {
                case SASTokenSubjects.Blob:
                    var blobMeta = await _blobMetaStore.GetAsync(token.BlobId);
                    if (blobMeta == null) return NotFound($"SAS subject blob {token.BlobId} not found.");
                    break;
                case SASTokenSubjects.Container:
                    var containerMeta = await _containerMetaStore.GetAsync(token.ContainerId);
                    if (containerMeta == null) return NotFound($"SAS subject container {token.ContainerId} not found.");
                    break;
            }

            token = await _sasStore.AddAsync(token);

            return Ok(token);
        }
    }
}
