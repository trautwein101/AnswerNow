using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class AuthResponseMappings
    {

        public static AuthResponseDto ToDto(this AuthResponse entity)
        {
            return new AuthResponseDto
            {
                AccessToken = entity.AccessToken,
                AccessTokenExpiration = entity.AccessTokenExpiration,
                RefreshToken = entity.RefreshToken,
                RefreshTokenExpiration = entity.RefreshTokenExpiration,
                UserId = entity.User.Id,
                Email = entity.User.Email,
                DisplayName = entity.User.DisplayName,
                Role = entity.User.Role.ToString()
            };
        }


        public static AuthResponse ToEntity(this AuthResponseDto dto)
        {
            return new AuthResponse
            {
                AccessToken = dto.AccessToken,
                AccessTokenExpiration = dto.AccessTokenExpiration,
                RefreshToken = dto.RefreshToken,
                RefreshTokenExpiration = dto.RefreshTokenExpiration
            };
        }


    }
}
