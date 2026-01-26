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
                IsInActive = user.IsInActive,
                IsSuspended = user.IsSuspended,
                IsPending = user.IsPending,
                IsBanned = user.IsBanned,
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
                Role = Enum.Parse<UserRole>(dto.Role, ignoreCase: true),
                IsProfessional = dto.IsProfessional,
                IsActive = dto.IsActive,
                IsInActive = dto.IsInActive,
                IsPending = dto.IsPending,
                IsSuspended = dto.IsSuspended,
                IsBanned = dto.IsBanned,
                LastLogin = dto.LastLogin ?? DateTime.UtcNow,
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
