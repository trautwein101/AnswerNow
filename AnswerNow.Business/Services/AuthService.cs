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

namespace AnswerNow.Business.Services
{
    public class AuthService : IAuthService
    {

        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public AuthService(IUserRepository userRepository, IConfiguration configuration, IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _refreshTokenRepository = refreshTokenRepository;
        }

        // Register New User
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
            {
                return null; //email is already taken
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                PasswordHash = passwordHash
            };

            var createdUser = await _userRepository.CreateAsync(user);

            //Generate JWT token for immediate login
            return await GenerateAuthResponse(createdUser);

        }

        //Login Existing User
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {

            var user = await _userRepository.GetByEmailAsync(dto.Email);


            if (user == null)
            {
                return null;
            }

            //extracts salt from hash and re-hashes
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return null;  // Wrong password
            }

            await _userRepository.UpdateLastLoginAsync(user.Id);

            return await GenerateAuthResponse(user);

        }

        //Logout Existing User ~ may be one device
        public async Task LogoutAsync(string refreshToken)
        {
            await _refreshTokenRepository.RevokeAsync(refreshToken, "user logged out");
        }

        //Logout Existing User
        public async Task LogoutEverywhereAsync(int userId)
        {
            await _refreshTokenRepository.RevokeAllForUserAsync(userId, "user logged out");
        }

        // JWT Token Generation - for both access token and refresh token
        private async Task<AuthResponseDto> GenerateAuthResponse(User user)
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
            var secretKey = _configuration["Jwt:Secretkey"];
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
            return new AuthResponseDto
            {
                AccessToken = accessToken,
                AccessTokenExpiration = accessTokenExpiration,
                RefreshToken = refreshToken,
                RefreshTokenExpiration = refreshTokenExpiration,
                UserId = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                Role = user.Role.ToString()
            };

        }

        //Refresh token
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

        //Refresh Token
        public async Task<AuthResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            //return the domain model
            var storedToken = await _refreshTokenRepository.GetByTokenAsync(dto.RefreshToken);

            if (storedToken == null || !storedToken.IsActive || storedToken.User == null)
            {
                return null;
            }

            //rotate the refresh token
            await _refreshTokenRepository.RevokeAsync(dto.RefreshToken, "Replaced by new token");

            //user is already a domain modle from the mapping
            return await GenerateAuthResponse(storedToken.User);

        }


    }

}
