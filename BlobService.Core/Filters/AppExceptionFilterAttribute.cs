using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlobService.Core.Filters
{
    internal class AppExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public AppExceptionFilterAttribute(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogCritical($"{context.ActionDescriptor.DisplayName} : {context.Exception.Message}");
        }
    }
}
