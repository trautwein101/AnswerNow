using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AnswerNow.Utilities.Exceptions
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
                await HandleExceptionAsync(context, ex);
            }
        }


        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            //note: method and path good for ops, traceId for correlation, and excpetion stack trace for logs only ~ no return to clients.
            _logger.LogError(ex, "Unhandled exception while processing request {Method} {Path}. TraceId={TraceId}", 
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier);

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var errorPayload = new
            {
                traceId = context.TraceIdentifier, //for correlating logs
                message = "An unexpected error occured. Please try again later."
            };

            var json = JsonSerializer.Serialize(errorPayload);

            await context.Response.WriteAsync(json);
        }


    }
}
