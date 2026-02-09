using Microsoft.AspNetCore.Http;

namespace AnswerNow.Utilities.Exceptions
{

    /// <summary>
    /// Base type for all application-level exceptions for business failures and not system crashes.
    /// </summary>
    public abstract class AppException : Exception
    {
        protected AppException(string message, Exception? inner = null)
            : base(message, inner) { }

        public abstract int StatusCode { get; }
        public virtual string Title => "Request failed.";
        public virtual string Type => "https://answernowplace.com/errors/400";
    }


    /// <summary>
    /// Thrown when a requested resource does not exist
    /// </summary>  
    public sealed class NotFoundAppException : AppException
    {
        public NotFoundAppException(string message) : base(message) { }

        public override int StatusCode => StatusCodes.Status404NotFound;
        public override string Title => "Resource not found.";
        public override string Type => "https://answernowplace.com/errors/404";
    }


    /// <summary>
    /// Thrown when a user is authenticated but not allowed to perform an action.
    /// </summary>
    public sealed class ForbiddenAppException : AppException
    {
        public ForbiddenAppException(string message) : base(message) { }

        public override int StatusCode => StatusCodes.Status403Forbidden;
        public override string Title => "Forbidden.";
        public override string Type => "https://answernowplace.com/errors/403";
    }


    /// <summary>
    /// Thrown when input data violates business rules.
    /// </summary>
    public sealed class ValidationAppException: AppException
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public ValidationAppException(
            string message,
            IReadOnlyDictionary<string, string[]> errors)
            : base(message)
        {
            Errors = errors;
        }

        public override int StatusCode => StatusCodes.Status400BadRequest;
        public override string Title => "Validation failed.";
        public override string Type => "https://answernowplace.com/errors/400";

    }


}
