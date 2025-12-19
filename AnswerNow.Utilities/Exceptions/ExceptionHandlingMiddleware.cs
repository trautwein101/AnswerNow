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
            _next = next; //next piece middleware / MVC pipeline.
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); //let rest of pipeline run (controllers, etc.)
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }


        public async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception while processing request {Path}", context.Request); //log full details on server side

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
