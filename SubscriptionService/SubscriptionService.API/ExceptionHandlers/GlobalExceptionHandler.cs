using Microsoft.AspNetCore.Diagnostics;
using SubscriptionService.BusinessLogic.Exceptions;
using SubscriptionService.BusinessLogic.Models;
using System.Net;

namespace SubscriptionService.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = GetErrorCode(exception);
            var problemDetails = new 
            {
                Status = statusCode,
                Title = exception.Message,
                Details = GetErrorDetails(exception)
            };

            httpContext.Response.StatusCode = statusCode;

            _logger.LogError(exception, $"Exception: {exception.GetType().Name} {exception.Message}");

            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }

        private static int GetErrorCode(Exception e)
        {
            switch (e)
            {
                case UnprocessableEntityException _:
                    return (int)HttpStatusCode.UnprocessableEntity;
                case NotFoundException _:
                    return (int)HttpStatusCode.NotFound;
                case AuthorizationException _:
                    return (int)HttpStatusCode.Unauthorized;
                case BadRequestException _:
                    return (int)HttpStatusCode.BadRequest;
                default:
                    return (int)HttpStatusCode.BadRequest;
            }
        }

        private static IEnumerable<ErrorDetail>? GetErrorDetails(Exception exception)
        {
            IEnumerable<ErrorDetail>? detail = null;

            switch (exception)
            {
                case ValidationPipelineException validationException:
                    detail = validationException.ValidationErrors;
                    break;
            }

            return detail;
        }
    }
}
