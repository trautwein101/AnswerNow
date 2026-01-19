using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IModeratorService
    {
        Task<ModeratorStats> GetModeratorStatsAsync();
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended);

    }
}
