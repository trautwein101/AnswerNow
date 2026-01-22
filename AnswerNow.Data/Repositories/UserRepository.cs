using AnswerNow.Data.Entities;
using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Mappings;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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

        //Admin methods
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var entities = await UserWithCountsQuery()
                .OrderByDescending(u => u.DateCreated)
                .ToListAsync();

            return entities.Select(e => e.ToDomain());
        }

        public async Task<User?> UpdateRoleAsync(int userId, UserRole newRole)
        {
            var entity = await UserWithCountsQuery()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (entity == null)
                return null;

            entity.Role = newRole;
            await _dbContext.SaveChangesAsync();

            return entity.ToDomain();
        }

        public Task<User?> UpdateUserStatusAsync(int userId, UserStatus newStatus) 
            => SetUserStatusAsync(userId, newStatus);


        private async Task<User?> SetUserStatusAsync(int userId, UserStatus newStatus)
        {
            var entity = await UserWithCountsQuery()
                .SingleOrDefaultAsync(u => u.Id == userId);

            if (entity == null) return null;

            //clean the current state
            entity.IsActive = false;
            entity.IsInActive = false;
            entity.IsPending = false;
            entity.IsSuspended = false;
            entity.IsBanned = false;

            //Update Status
            switch (newStatus)
            {
                case UserStatus.Active:
                    entity.IsActive = true;
                    break;
                case UserStatus.InActive:
                    entity.IsInActive = true;
                    break;
                case UserStatus.Pending:
                    entity.IsPending = true;
                    break;
                case UserStatus.Suspended:
                    entity.IsSuspended = true;
                    break;
                case UserStatus.Banned:
                    entity.IsBanned = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newStatus), newStatus, "Unsupported status");
            }

            await _dbContext.SaveChangesAsync();

            return entity.ToDomain();

        }

        private IQueryable<UserEntity> UserWithCountsQuery()
            => _dbContext.Users
            .Include(u => u.Questions)
            .Include(u => u.Answers);

        //Totals
        public async Task<int> GetTotalCountAsync()
        {
            return await _dbContext.Users.CountAsync();
        }

        public async Task<int> GetNewUsersCountAsync(int days)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Users.CountAsync(u => u.DateCreated >= cutOffDate);

        }

    }
}
