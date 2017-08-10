using BlobService.Core.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace BlobService.Core.Security
{
    public class RequestAccessDescriptor
    {
        public SASTokenSubjects TokenSubject { get; set; }
        public string BlobId { get; set; }
        public BlobAccessTypes BlobAccessType { get; set; }
        public string ContainerId { get; set; }
        public ContainerAccessTypes ContainerAccessType { get; set; }
    }

    public class ValidateSASTokenFilter : ActionFilterAttribute
    {
        protected readonly BlobServiceOptions _options;
        protected readonly ILogger _logger;
        protected readonly ISASStore _sasStore;
        protected readonly IContainerStore _containerStore;
        protected readonly IBlobStore _blobStore;

        public ValidateSASTokenFilter(
            BlobServiceOptions options,
            ILoggerFactory loggerFactory,
            ISASStore sasStore,
            IContainerStore containerStore,
            IBlobStore blobStore)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _logger = loggerFactory?.CreateLogger<ValidateSASTokenFilter>() ?? throw new ArgumentNullException(nameof(loggerFactory));
            _sasStore = sasStore ?? throw new ArgumentNullException(nameof(sasStore));
            _containerStore = containerStore ?? throw new ArgumentNullException(nameof(containerStore));
            _blobStore = blobStore ?? throw new ArgumentNullException(nameof(blobStore));
        }


        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (_options.RequireSASAuthorization == false) return;

            var sasKeyStringValues = context.HttpContext.Request.Query[Constants.SASQueryStringKey];

            if (sasKeyStringValues.Count != 1)
            {
                context.Result = new BadRequestObjectResult($"Missing or multiple {Constants.SASQueryStringKey} in querystring.");
                return;
            }

            var sasKey = sasKeyStringValues.ToString();
            var sasToken = await _sasStore.GetAsync(sasKey);

            if (sasToken == null)
            {
                context.Result = new BadRequestObjectResult($"Requested SASToken {sasKey} not found.");
                return;
            }

            var accessDescriptor = GetAccessDescriptor(context);

            await base.OnActionExecutionAsync(context, next);
        }


        private RequestAccessDescriptor GetAccessDescriptor(ActionExecutingContext context)
        {
            var descriptor = new RequestAccessDescriptor();

            return descriptor;
        }

    }
}
