using AnswerNow.Data.Repositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.IRepositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<User> CreateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateLastLoginAsync(int userId);

        //Admin Methods
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateRoleAsync(int userId, UserRole newRole);
        Task<int> GetTotalCountAsync();
        Task<int> GetNewUsersCountAsync(int days);

        //Admin & Moderator Methods
        Task<User?> UpdateUserStatusAsync(int userId, UserStatus newStatus);


    }
}
