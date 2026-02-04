using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    /// <summary>
    /// Provides administrative operations for managing users and platform-wide data.
    /// </summary>
    public interface IAdminService
    {

        /// <summary>
        /// Retrieves administrative dashboard statistics.
        /// </summary>
        Task<AdminStats> GetAdminStatsAsync();

        /// <summary>
        /// Retrieves all users (admin view).
        /// </summary>
        Task<IEnumerable<User>> GetAllUsersAsync();

        /// <summary>
        /// Changes a user's role.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        /// <param name="newRole">The new role value.</param>
        /// <returns>
        /// The updated <see cref="User"/> if successful, or null if invalid or user not found.
        /// </returns>
        Task<User?> ChangeUserRoleAsync(int userId, string newRole);

        /// <summary>
        /// Sets a user's active status.
        /// </summary>
        Task<User?> SetUserActiveStatusAsync(int userId, bool isActive);

        /// <summary>
        /// Sets a user's inactive status.
        /// </summary>
        Task<User?> SetUserInActiveStatusAsync(int userId, bool isInActive);

        /// <summary>
        /// Sets a user's pending status.
        /// </summary>
        Task<User?> SetUserPendingStatusAsync(int userId, bool isPending);

        /// <summary>
        /// Sets a user's banned status.
        /// </summary>
        Task<User?> SetUserBanStatusAsync(int userId, bool isBanned);

        /// <summary>
        /// Sets a user's suspended status.
        /// </summary>
        Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended);

    }
}
