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

        public AppExceptionFilterAttribute(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<AppExceptionFilterAttribute>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.LogCritical($"{context.ActionDescriptor.DisplayName} : {context.Exception.Message}");
        }
    }
}
