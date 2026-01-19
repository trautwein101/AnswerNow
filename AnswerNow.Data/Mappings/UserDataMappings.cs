using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.Mappings
{
    public static class UserDataMappings
    {
        public static User ToDomain(this UserEntity entity)
        {
            return new User
            {
                Id = entity.Id,
                Email = entity.Email,
                DisplayName = entity.DisplayName,
                PasswordHash = entity.PasswordHash,
                Role = entity.Role,
                IsProfessional = entity.IsProfessional,
                IsActive = entity.IsActive,
                IsBanned = entity.IsBanned,
                IsSuspended = entity.IsSuspended,
                LastLogin = entity.LastLogin,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated,
                QuestionCount = entity.Questions?.Count ?? 0,
                AnswerCount = entity.Answers?.Count ?? 0
            };
        }

        public static UserEntity ToEntity(this User domain)
        {
            return new UserEntity
            {
                Id = domain.Id,
                Email = domain.Email,
                DisplayName = domain.DisplayName,
                PasswordHash = domain.PasswordHash,
                Role = domain.Role,
                IsProfessional= domain.IsProfessional,
                IsActive = domain.IsActive,
                IsBanned = domain.IsBanned,
                IsSuspended = domain.IsSuspended,
                LastLogin = domain.LastLogin,
                DateCreated = domain.DateCreated,
                DateUpdated = domain.DateUpdated,           
            };
        }

    }
}
