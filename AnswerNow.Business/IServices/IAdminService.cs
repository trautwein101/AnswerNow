using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IAdminService
    {
       
        Task<AdminStats> GetAdminStatsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> ChangeUserRoleAsync(int userId, string newRole);
        Task<User?> SetUserActiveStatusAsync(int userId, bool isActive);
        Task<User?> SetUserInActiveStatusAsync(int userId, bool isInActive);
        Task<User?> SetUserPendingStatusAsync(int userId, bool isPending);
        Task<User?> SetUserBanStatusAsync(int userId, bool isBanned);
        Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended);


    }
}
