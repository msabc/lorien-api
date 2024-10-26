using Lorien.Application.Exceptions;
using Lorien.Application.Models.Response.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Lorien.Api.Filters
{
    internal sealed class ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> _logger) : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            _logger.LogError("An unexpected error has occurred: ", context.Exception.Message);

            if (context.Exception is CustomHttpException customHttpException)
            {
                context.HttpContext.Response.StatusCode = (int)customHttpException.StatusCode.Value;
                var errorObject = new HttpExceptionResponse()
                {
                    Message = customHttpException.Message
                };

                context.Result = new ObjectResult(errorObject);
            }

            base.OnException(context);
        }
    }
}
