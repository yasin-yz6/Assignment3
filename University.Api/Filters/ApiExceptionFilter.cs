using AutoWrapper.Wrappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using University.Core.Exceptions;

namespace University.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (context.Exception is NotFoundException)
            {
                _logger.LogError(context.Exception, "Item not found");
                context.Result = Response(context.Exception.Message, "Item not found", StatusCodes.Status404NotFound);
                return;
            }
            if (context.Exception is BusinessException businessException)
            {
                if (businessException.Errors.Any())
                {
                    _logger.LogError(context.Exception, "One or more business validations error occurred");
                    context.Result = Response(businessException.Errors, "One or more business validations error occurred", StatusCodes.Status400BadRequest);
                    return;
                }
                else
                {
                    _logger.LogError(context.Exception, "One or more business validations error occurred");
                    context.Result = Response(businessException.Message, "One or more business validations error occurred", StatusCodes.Status400BadRequest);
                    return;
                }
                
            }
            if (context.Exception is ArgumentNullException)
            {
                _logger.LogError(context.Exception, "Missing data");
                context.Result = Response(context.Exception.Message, "Missing data", StatusCodes.Status400BadRequest);
                return;
            }
            if (context.Exception is UnauthorizedAccessException)
            {
                _logger.LogError(context.Exception, "Unauthorized access");
                context.Result = Response(context.Exception.Message, "Unauthorized", StatusCodes.Status403Forbidden);
                return;
            }
            _logger.LogError(context.Exception, "Unhandled exception occurred");
            context.Result = Response(context.Exception.Message, "Internal Server Error", StatusCodes.Status500InternalServerError, context.Exception.StackTrace);
            return;
        }

        public ObjectResult Response(string message, string title, int status, string? stackTrace = null)
        {
            var result = new ApiResponse
            {
                StatusCode = status,
                Message = message,
                IsError = true,
                Version = "1.0",
                Result = stackTrace
            };
            return new ObjectResult(result)
            {
                StatusCode = status
            };
        }
        public ObjectResult Response(Dictionary<string, List<string>> errors, string title, int status)
        {
            var result = new ApiResponse
            {
                StatusCode = status,
                Message = title,
                ResponseException = title,
                IsError= true,
                Version = "1.0",
                Result = errors
            };
            return new ObjectResult(result)
            {
                StatusCode = status
            };
        }
    }
}
