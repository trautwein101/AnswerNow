using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IAdminService
    {
       
        Task<User> GetStatsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> ChangeUserRoleAsync(int userId, string newRole);
        Task<User?> SetUserBanStatusAsync(int userId, bool isBanned);

    }
}
