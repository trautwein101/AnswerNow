using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class UserMappings
    {

        public static UserDto ToDto(this User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                DisplayName = user.DisplayName,
                IsBanned = user.IsBanned,
                IsActive = user.IsActive,
                Role = user.Role.ToString(),
                DateCreated = user.DateCreated
            };
        }


        public static User ToEntity(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                IsBanned = dto.IsBanned,
                IsActive = dto.IsActive,
                Role = Enum.Parse<UserRole>(dto.Role),
                DateCreated = dto.DateCreated
            };
        }
            


    }
}
