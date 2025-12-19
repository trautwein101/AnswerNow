using Microsoft.AspNetCore.Builder;
using AnswerNow.Utilities.Exceptions;

namespace AnswerNow.Utilities.Extensions
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }


    }
}
