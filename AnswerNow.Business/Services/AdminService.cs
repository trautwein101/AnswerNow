using AnswerNow.Business.IServices;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace AnswerNow.Business.Services
{
    public class AdminService : IAdminService
    {

        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;

        public AdminService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IUserRepository userRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
        }

        /// <inheritdoc />
        public async Task<AdminStats> GetAdminStatsAsync()
        {
            
            // todo: run in parallel for optimization
            var totalUsers = await _userRepository.GetTotalCountAsync();
            var totalQuestions = await _questionRepository.GetTotalCountAsync();
            var totalAnswers = await _answerRepository.GetTotalCountAsync();
            var newUsersThisWeek = await _userRepository.GetNewUsersCountAsync(7);
            var newQuestionsThisWeek = await _questionRepository.GetNewQuestionsCountAsync(7);
            var newAnswersThisWeek = await _answerRepository.GetNewAnswersCountAsync(7);
            var totalIsFlaggedQuestions = await _questionRepository.GetTotalIsFlaggedCountAsync();
            var totalIsFlaggedAnswers = await _answerRepository.GetTotalIsFlaggedCountAsync();
            var newIsFlaggedQuestionsThisWeek = await _questionRepository.GetNewIsFlaggedCountAsync(7);
            var newIsFlaggedAnswersThisWeek = await _answerRepository.GetNewIsFlaggedCountAsync(7);

            return new AdminStats
            {
                TotalUsers = totalUsers,
                TotalQuestions = totalQuestions,
                TotalAnswers = totalAnswers,
                NewUsersThisWeek = newUsersThisWeek,
                NewQuestionsThisWeek = newQuestionsThisWeek,
                NewAnswersThisWeek = newAnswersThisWeek,
                TotalIsFlaggedQuestions = totalIsFlaggedQuestions,
                TotalIsFlaggedAnswers = totalIsFlaggedAnswers,
                NewIsFlaggedQuestionsThisWeek = newIsFlaggedQuestionsThisWeek,
                NewIsFlaggedAnswersThisWeek = newIsFlaggedAnswersThisWeek
            };

        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        //Business logic: promote and demote
        public async Task<User?> ChangeUserRoleAsync(int userId, string newRole)
        {
            //convert string to enum for update
            if(!Enum.TryParse<UserRole>(newRole, true, out var role))
            {
                return null; // invalid string provided
            }

            var updatedUser = await _userRepository.UpdateRoleAsync(userId, role);

            if(updatedUser == null)
            {
                return null;
            }

            return updatedUser;

        }

        /// <inheritdoc />
        public async Task<User?> SetUserActiveStatusAsync(int userId, bool isActive)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.Active, isActive);
            return await _userRepository.UpdateUserStatusAsync(userId, newStatus);

        }

        /// <inheritdoc />
        public async Task<User?> SetUserInActiveStatusAsync(int userId, bool isInActive)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.InActive, isInActive);
            return await _userRepository.UpdateUserStatusAsync(userId, newStatus);

        }

        /// <inheritdoc />
        public async Task<User?> SetUserPendingStatusAsync(int userId, bool isPending)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.Pending, isPending);
            return await _userRepository.UpdateUserStatusAsync(userId, newStatus);

        }

        /// <inheritdoc />
        public async Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.Suspended, isSuspended);
            return await _userRepository.UpdateUserStatusAsync(userId, newStatus);

        }

        /// <inheritdoc />
        public async Task<User?> SetUserBanStatusAsync(int userId, bool isBanned)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.Banned, isBanned);
            return await _userRepository.UpdateUserStatusAsync(userId, newStatus);

        }

        private UserStatus GetNewStatus(User user, UserStatus status, bool value)
        {

            //true(turn on) will always force the status
            if (value) return status;

            var currentState = GetCurrentStatus(user);

            //false(turn off) only changes if currently in that state
            return status switch
            {
                UserStatus.Active => currentState == UserStatus.Active ? UserStatus.InActive : currentState,
                UserStatus.InActive => currentState == UserStatus.InActive ? UserStatus.Active : currentState,
                UserStatus.Pending => currentState == UserStatus.Pending ? UserStatus.Active : currentState,
                UserStatus.Suspended => currentState == UserStatus.Suspended ? UserStatus.Active : currentState,
                UserStatus.Banned => currentState == UserStatus.Banned ? UserStatus.Active : currentState,
                _ => throw new ArgumentOutOfRangeException(nameof(status), status, "Unsupported status")
            };
        }

        private static UserStatus GetCurrentStatus(User u)
        {
            if (u.IsBanned) return UserStatus.Banned;
            if (u.IsSuspended) return UserStatus.Suspended;
            if (u.IsPending) return UserStatus.Pending;
            if (u.IsInActive) return UserStatus.InActive;
            return UserStatus.Active;
        }


    }
}
