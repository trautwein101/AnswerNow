using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace AnswerNow.Utilities.Exceptions
{
    //RFC-7807 ProblemDetails
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


        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            _logger.LogError(
                ex,
                "Unhandled exception while processing request {Method} {Path}. TraceId={TraceId}",
                context.Request.Method,
                context.Request.Path,
                context.TraceIdentifier
                );

            var problem = CreateProblemDetails(context, ex);

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = problem.Status ?? StatusCodes.Status500InternalServerError;

            var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, jsonOptions));

        }


        private ProblemDetails CreateProblemDetails(HttpContext context, Exception ex)
        {

            //1. Validation errors ~ 400 + field errors
            if (ex is ValidationAppException vex)
            {
                var vpd = new ValidationProblemDetails(
                    vex.Errors.ToDictionary(k => k.Key, v => v.Value))
                {
                    Status = vex.StatusCode,
                    Title = vex.Title,
                    Type = vex.Type,
                    Detail = vex.Message,
                    Instance = context.Request.Path
                };

                vpd.Extensions["traceId"] = context.TraceIdentifier;
                AddActivityId(vpd);

                return vpd;

            }

            //2. Unkown application exceptions
            if (ex is AppException appEx)
            {
                var pd = new ProblemDetails
                {
                    Status = appEx.StatusCode,
                    Title = appEx.Title,
                    Type = appEx.Type,
                    Detail = appEx.Message,
                    Instance = context.Request.Path
                };

                pd.Extensions["traceId"] = context.TraceIdentifier;
                AddActivityId(pd);
                return pd;
            }

            //3. Fallback 500 errors
            var fallback = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occured.",
                Type = "https://answernowplace.com/errors/500",
                Detail = "Please try again later.",
                Instance = context.Request.Path
            };

            fallback.Extensions["traceId"] = context.TraceIdentifier;
            AddActivityId(fallback);

            return fallback;

        }


        private static void AddActivityId(ProblemDetails problem)
        {
            if (!string.IsNullOrWhiteSpace(Activity.Current?.Id))
            {
                problem.Extensions["activityId"] = Activity.Current.Id;
            }
        }

    }
}
