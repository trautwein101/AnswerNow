using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Mappings;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace AnswerNow.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {

        private readonly AnswerNowDbContext _dbContext;

        public RefreshTokenRepository(AnswerNowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            var entity = await _dbContext.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Token == token);

            return entity?.ToDomain();
        }

        public async Task<RefreshToken?> CreateAsync(RefreshToken refreshToken)
        {
            var entity = refreshToken.ToEntity();
            entity.CreatedAt = DateTime.UtcNow;

            _dbContext.RefreshTokens.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity?.ToDomain();
        }

        public async Task RevokeAsync(string token, string reason)
        {
            var entity = await _dbContext.RefreshTokens.FirstOrDefaultAsync(r => r.Token == token);

            if (entity != null)
            {
                entity.IsRevoked = true;
                entity.RevokedAt = DateTime.UtcNow;
                entity.RevokedReason = reason;
            }

            await _dbContext.SaveChangesAsync();

        }

        public async Task RevokeAllForUserAsync(int userId, string reason)
        {
            var entities = await _dbContext.RefreshTokens.Where(r => r.UserId == userId && !r.IsRevoked).ToListAsync();

            foreach(var entity in entities)
            {
                entity.IsRevoked = true;
                entity.RevokedAt = DateTime.UtcNow;
                entity.RevokedReason = reason;
            }

            await _dbContext.SaveChangesAsync();

        }
    }
}
