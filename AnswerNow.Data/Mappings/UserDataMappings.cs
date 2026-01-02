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
                LastLogin = entity.LastLogin,
                IsActive = entity.IsActive,
                Role = entity.Role,
                IsBanned = entity.IsBanned,
                DateCreated = entity.DateCreated,
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
                LastLogin = domain.LastLogin,
                IsActive = domain.IsActive,
                Role = domain.Role,
                IsBanned = domain.IsBanned,
                DateCreated = domain.DateCreated
            };
        }

    }
}
