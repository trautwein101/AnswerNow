using AnswerNow.Domain.Models;

namespace AnswerNow.Data.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task<RefreshToken?> CreateAsync(RefreshToken refreshToken);
        Task RevokeAsync(string token, string reason);
        Task RevokeAllForUserAsync(int userId, string reason);
    }
}
