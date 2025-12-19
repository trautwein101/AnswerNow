using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Mappings;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AnswerNow.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AnswerNowDbContext _dbContext;

        public UserRepository(AnswerNowDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var entity = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
              
             return entity?.ToDomain();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Users.FindAsync(id);
            
            return entity?.ToDomain();
        }

        public async Task<User> CreateAsync(User user)
        {
            var entity = user.ToEntity();
            entity.DateCreated = DateTime.UtcNow;

            _dbContext.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.ToDomain();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            var entity = await _dbContext.Users.FindAsync(userId);
            if (entity != null)
            {
                entity.LastLogin = DateTime.Now;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
