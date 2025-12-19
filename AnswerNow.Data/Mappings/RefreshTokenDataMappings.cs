using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.Mappings
{
    public static class RefreshTokenDataMappings
    {
        //Entity to Domain MOdel
        public static RefreshToken ToDomain(this RefreshTokenEntity entity)
        {
            return new RefreshToken
            {
                Id = entity.Id,
                Token = entity.Token,
                UserId = entity.UserId,
                ExpiresAt = entity.ExpiresAt,
                CreatedAt = entity.CreatedAt,
                IsRevoked = entity.IsRevoked,
                RevokedAt = entity.RevokedAt,
                RevokedReason = entity.RevokedReason,
                DeviceInfo = entity.DeviceInfo,
                User = entity.User?.ToDomain() // Convert user too if loaded
            };
        }

        //Domain to Entity
        public static RefreshTokenEntity ToEntity(this RefreshToken domain)
        {
            return new RefreshTokenEntity
            {
                Id = domain.Id,
                Token = domain.Token,
                UserId = domain.UserId,
                ExpiresAt = domain.ExpiresAt,
                CreatedAt = domain.CreatedAt,
                IsRevoked = domain.IsRevoked,
                RevokedAt = domain.RevokedAt,
                RevokedReason = domain.RevokedReason,
                DeviceInfo = domain.DeviceInfo
            };
        }
    
    }
}
