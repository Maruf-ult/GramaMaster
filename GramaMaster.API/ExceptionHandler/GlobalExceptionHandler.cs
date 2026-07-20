using GramaMaster.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GramaMaster.API.ExceptionHandler
{
    public class GlobalExceptionHandler:IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool>TryHandleAsync(HttpContext httpContext,Exception exception,CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);

            var response = new ProblemDetails
            {
                Title = exception.Message
            };

            switch (exception)
            {
                case BadHttpRequestException:
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Bad Request";
                    break;
                case UnauthorizedException:
                    response.Status = StatusCodes.Status401Unauthorized;
                    response.Title = "Unauthorized";
                    break;
                case ForbiddenException:
                    response.Status = StatusCodes.Status403Forbidden;
                    response.Title = "Forbidden";
                    break;
                case NotFoundException:
                    response.Status = StatusCodes.Status404NotFound;
                    response.Title = "Not Found";
                    break;
                default:
                    response.Status = StatusCodes.Status500InternalServerError;
                    response.Title = "Internal Server Error";
                    break;
            }

            response.Detail = exception.Message;
            response.Instance = httpContext.Request.Path;

            httpContext.Response.StatusCode = response.Status.Value;

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);

            return true;
        }
    }
}
