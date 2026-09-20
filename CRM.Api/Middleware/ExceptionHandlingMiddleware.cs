using CRM.Application.Exceptions;
using System.Net;
using System.Text.Json;

namespace CRM.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = Map(ex);

                if (statusCode == HttpStatusCode.InternalServerError)
                    _logger.LogError(ex, "Unhandled exception at {Path}", context.Request.Path);
                else
                    _logger.LogWarning("{Type}: {Message} at {Path}", ex.GetType().Name, ex.Message, context.Request.Path);

                context.Response.Clear();
                context.Response.StatusCode = (int)statusCode;

                bool isApiRequest = context.Request.Path.StartsWithSegments("/api");

                if (isApiRequest)
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        status = (int)statusCode,
                        message
                    }));
                }
                else
                {
                    context.Response.Redirect($"/Home/Error?code={(int)statusCode}");
                }
            }
        }

        private static (HttpStatusCode, string) Map(Exception ex) => ex switch
        {
            NotFoundException => (HttpStatusCode.NotFound, ex.Message),
            ValidationException => (HttpStatusCode.BadRequest, ex.Message),
            _ => (HttpStatusCode.InternalServerError, "Internal server error")
        };
    }
}
