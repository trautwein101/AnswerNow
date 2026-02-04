using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{

    /// <summary>
    /// Provides access to the currently authenticated user context.
    /// </summary>
    public interface ICurrentUserService
    {
        /// <summary>
        /// Retrieves the current user based on the active request context.
        /// </summary>
        /// <returns>
        /// A <see cref="CurrentUser"/> representing the authenticated user.
        /// </returns>
        CurrentUser Get();

    }
}
