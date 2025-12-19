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
                DateCreated = entity.DateCreated,
                LastLogin = entity.LastLogin,
                IsActive = entity.IsActive,
                Role = entity.Role
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
                DateCreated = domain.DateCreated,
                LastLogin = domain.LastLogin,
                IsActive = domain.IsActive,
                Role = domain.Role
            };
        }

    }
}
