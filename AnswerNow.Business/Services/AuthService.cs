using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Repositories;
using AnswerNow.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace AnswerNow.Business.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<AuthResponse?> RegisterAsync(Register register)
        {

            _logger.LogInformation("Registration attempt. Email={Email}", register.Email);

            if (await _userRepository.EmailExistsAsync(register.Email))
            {
                _logger.LogWarning("Registration failed: email already exists. Email={Email}", register.Email);

                return null; //email is already taken
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(register.Password);

            var user = new User
            {
                Email = register.Email,
                DisplayName = register.DisplayName,
                PasswordHash = passwordHash
            };

            var createdUser = await _userRepository.CreateAsync(user);

            _logger.LogInformation("Registration succeeded. UserId={UserId}", createdUser.Id);

            //Generate JWT token for immediate login
            return await GenerateAuthResponse(createdUser);

        }

        /// <inheritdoc />
        public async Task<AuthResponse?> LoginAsync(Login login)
        {

            _logger.LogInformation("Login attempt. Email={Email}", login.Email);

            var user = await _userRepository.GetByEmailAsync(login.Email);


            if (user == null)
            {
                _logger.LogWarning("Login failed: user not found. Email={Email}", login.Email);
                return null;
            }

            //extracts salt from hash and re-hashes
            if (!BCrypt.Net.BCrypt.Verify(login.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: invalid password. UserId={UserId}", user.Id);
                return null;  // Wrong password
            }

            await _userRepository.UpdateLastLoginAsync(user.Id);

            _logger.LogInformation("Login succeeded. UserId={UserId}", user.Id);

            return await GenerateAuthResponse(user);

        }

        /// <inheritdoc />
        public async Task LogoutAsync(string refreshToken)
        {
            _logger.LogInformation("Logout attempt.");
            await _refreshTokenRepository.RevokeAsync(refreshToken, "user logged out");
            _logger.LogInformation("Logout succeeded.");
        }

        /// <inheritdoc />
        public async Task LogoutEverywhereAsync(int userId)
        {
            _logger.LogInformation("Logout everywhere attempt. UserId={UserId}", userId);
            await _refreshTokenRepository.RevokeAllForUserAsync(userId, "user logged out");
            _logger.LogInformation("Logout everywhere succeeded. UserId={UserId}", userId);
        }

        private async Task<AuthResponse> GenerateAuthResponse(User user)
        {
   
            //Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.DisplayName),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            //Signing Key
            var secretKey = _configuration["Jwt:SecretKey"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Token Expiration ~ 15 minutes
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(15);

            //Create the JWT Token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: accessTokenExpiration,
                signingCredentials: credentials
            );

            //Serialize the string
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            //Refresh token ~ 7 days
            var refreshToken = await GenerateRefreshToken(user.Id);

            //Refresh token expiration
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(
                _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7));

            //Return both tokens
            return new AuthResponse
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration,
                User = user
            };

        }

        private async Task<string> GenerateRefreshToken(int userId)
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            var token = Convert.ToBase64String(randomBytes);

            var refreshToken = new RefreshToken
            {
                Token = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(
                    _configuration.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7)),
                DeviceInfo = "Unknown"
            };

            await _refreshTokenRepository.CreateAsync(refreshToken);

            return token;

        }

        /// <inheritdoc />
        public async Task<AuthResponse?> RefreshTokenAsync(string refreshToken)
        {
            _logger.LogInformation("Refresh token attempt.");

            //return the domain model
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(refreshToken);

            if (storedToken == null || !storedToken.IsActive || storedToken.User == null)
            {
                _logger.LogWarning("Refresh token failed: invalid or inactive token.");

                return null;
            }

            //rotate the refresh token
            await _refreshTokenRepository.RevokeAsync(refreshToken, "Replaced by new token");

            _logger.LogInformation("Refresh token succeeded. UserId={UserId}", storedToken.User.Id);

            //user is already a domain model from the mapping
            return await GenerateAuthResponse(storedToken.User);

        }


    }

}
