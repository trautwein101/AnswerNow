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
                Role = user.Role.ToString(),
                IsProfessional = user.IsProfessional,
                IsActive = user.IsActive,
                IsBanned = user.IsBanned,
                IsSuspended = user.IsSuspended,
                LastLogin = user.LastLogin,
                DateCreated = user.DateCreated,
                DateUpdated = user.DateUpdated,
                QuestionCount = user.QuestionCount,
                AnswerCount = user.AnswerCount,
                QuestionFlagCount = user.QuestionFlagCount,
                AnswerFlagCount = user.AnswerFlagCount
            };
        }


        public static User ToEntity(this UserDto dto)
        {
            return new User
            {
                Id = dto.Id,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                Role = Enum.Parse<UserRole>(dto.Role),
                IsProfessional = dto.IsProfessional,
                IsActive = dto.IsActive,
                IsBanned = dto.IsBanned,
                IsSuspended = dto.IsSuspended,
                LastLogin = dto.LastLogin,
                DateCreated = dto.DateCreated,
                DateUpdated = dto.DateUpdated,
                QuestionCount = dto.QuestionCount,
                AnswerCount = dto.AnswerCount,
                QuestionFlagCount = dto.QuestionFlagCount,
                AnswerFlagCount = dto.AnswerFlagCount
            };
        }
            


    }
}
