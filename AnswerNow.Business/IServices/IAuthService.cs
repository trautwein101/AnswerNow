using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{

    /// <summary>
    /// Provides authentication and authorization-related operations.
    /// </summary>
    public interface IAuthService
    {

        /// <summary>
        /// Registers a new user account.
        /// </summary>
        /// <param name="register">The registration payload.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> if registration succeeds, or null if the email already exists.
        /// </returns>
        Task<AuthResponse?> RegisterAsync(Register register);

        /// <summary>
        /// Authenticates an existing user.
        /// </summary>
        /// <param name="login">The login credentials.</param>
        /// <returns>
        /// An <see cref="AuthResponse"/> if authentication succeeds, or null if credentials are invalid.
        /// </returns>
        Task<AuthResponse?> LoginAsync(Login login);

        /// <summary>
        /// Exchanges a valid refresh token for a new authentication response.
        /// </summary>
        /// <param name="refreshToken">The refresh token.</param>
        /// <returns>
        /// A new <see cref="AuthResponse"/> if the token is valid, or null if invalid or expired.
        /// </returns>
        Task<AuthResponse?> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Logs out a user session by revoking the specified refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to revoke.</param>
        Task LogoutAsync(string refreshToken);

        /// <summary>
        /// Logs out a user from all devices by revoking all refresh tokens.
        /// </summary>
        /// <param name="userId">The unique ID of the user.</param>
        Task LogoutEverywhereAsync(int userId);
    }
}
