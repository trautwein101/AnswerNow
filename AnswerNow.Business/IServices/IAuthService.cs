using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IAuthService
    {
        Task<AuthResponse?> RegisterAsync(Register register);
        Task<AuthResponse?> LoginAsync(Login login);
        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);
        Task LogoutAsync(string refreshToken);
        Task LogoutEverywhereAsync(int userId);
    }
}
