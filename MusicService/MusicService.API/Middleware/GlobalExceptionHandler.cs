using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MusicService.Domain.Exceptions;
using System.Net;

namespace MusicService.API.Middleware
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
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.Message,
            };

            httpContext.Response.StatusCode = statusCode;

            _logger.LogError(exception, $"Exception: {exception.GetType().Name}. {exception.Message}");

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
    }
}
