﻿using Identity.BusinessLogic.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

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
                Title = exception.Message,
                Detail = GetProblemDetails(exception)
            };

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }

        private string GetProblemDetails(Exception e)
        {
            switch (e)
            {
                case BaseIdentityException identityException:
                    
                    string detail =  JsonSerializer.Serialize(
                        identityException.Errors?.Select(e => new { Error = e.Description }));
                    return detail;
                default:
                    return string.Empty;
            }
        }

        private static int GetErrorCode(Exception e)
        {
            switch (e)
            {
                case UnprocessableEntityException _:
                    return (int)HttpStatusCode.UnprocessableEntity;
                case NotFoundException _:
                    return (int)HttpStatusCode.NotFound;
                case InvalidAuthorizationException _:
                    return (int)HttpStatusCode.Unauthorized;
                default:
                    return (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
