
namespace SubscriptionService.API.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var query = context.Request.QueryString;
            var request = $"{method} {path}{query}";

            _logger.LogInformation($"Request starting: {request}. ");

            await _next(context);

            var statusCode = context.Response.StatusCode;
            var contentLength = context.Response.ContentLength ?? 0;

            _logger.LogInformation($"Request finished: {request}, {statusCode}, content length: {contentLength}");

        }
    }
}