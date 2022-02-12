using Serilog;
using Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Web.Api.Exceptions;

namespace Web.Api.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        public ApiExceptionFilter(ILogger logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            _logger.Error(context.Exception, "ApiExceptionFilter");

            if (context.Exception is NotFoundItemException)
            {
                context.Result = new NotFoundResult();
            }
            else if (context.Exception is PostNotFoundException)
            {
                context.Result = new NotFoundResult();
            }
            else if (context.Exception is InvalidTokenException)
            {
                context.Result = new UnauthorizedResult();
            }

            if (context.Result == null)
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);

            base.OnException(context);
        }
    }
}
