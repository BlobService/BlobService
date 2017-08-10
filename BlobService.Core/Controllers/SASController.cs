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
        protected readonly IContainerStore _containerStore;
        protected readonly IBlobStore _blobStore;

        public SASController(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            ISASStore sasStore,
            IContainerStore containerStore,
            IBlobStore blobStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<ContainersController>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _sasStore = sasStore ?? throw new ArgumentNullException(nameof(sasStore));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
            _blobStore = blobStore ?? throw new ArgumentNullException(nameof(blobStore));
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

            switch (token.TokenSubject)
            {
                case SASTokenSubjects.Blob:
                    var blob = await _blobStore.GetByIdAsync(token.BlobId);
                    if (blob == null) return NotFound($"SAS subject blob {token.BlobId} not found.");
                    break;
                case SASTokenSubjects.Container:
                    var container = await _containerStore.GetAsync(token.ContainerId);
                    if (container == null) return NotFound($"SAS subject container {token.ContainerId} not found.");
                    break;
            }

            token = await _sasStore.AddAsync(token);

            return Ok(token);
        }
    }
}
