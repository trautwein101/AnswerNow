using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{

    /// <summary>
    /// Provides moderation-related operations for managing users and reviewing platform activity.
    /// </summary>
    public interface IModeratorService
    {

        /// <summary>
        /// Retrieves summary statistics for the moderator dashboard.
        /// </summary>
        /// <returns>
        /// A <see cref="ModeratorStats"/> object containing moderation metrics,
        /// or null if statistics are not available.
        /// </returns>
        Task<ModeratorStats> GetModeratorStatsAsync();

        /// <summary>
        /// Retrieves all users visible to moderators.
        /// </summary>
        /// <returns>
        /// A collection of <see cref="User"/> domain models.
        /// </returns>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Suspends or unsuspends a user.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="isSuspended">True to suspend the user; false to remove suspension.</param>
        /// <returns>
        /// The updated <see cref="User"/> if the operation succeeds, or null if the user does not exist.
        /// </returns>
        Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended);

    }
}
