using Microsoft.AspNetCore.Builder;
using AnswerNow.Utilities.Exceptions;

namespace AnswerNow.Utilities.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        //RFC-7807 ProblemDetails
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }


    }
}
