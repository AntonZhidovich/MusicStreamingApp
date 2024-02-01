using Identity.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Identity.API.ExceptionHandlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            int statusCode = GetErrorCode(exception);
            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = exception.Message
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }

        private static int GetErrorCode(Exception e)
        {
            switch (e)
            {
                case ArgumentNullException _:
                    return (int)HttpStatusCode.UnprocessableEntity;
                case ArgumentException _:
                    return (int)HttpStatusCode.NotFound;
                case InvalidAuthorizationException _:
                    return (int)HttpStatusCode.Unauthorized;
                default:
                    return (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
