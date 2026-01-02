using AnswerNow.Business.IServices;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

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

        public async Task<AdminStats> GetStatsAsync()
        {
            
            // todo: run in parallel for optimization
            var totalUsers = await _userRepository.GetTotalCountAsync();
            var totolQuestions = await _questionRepository.GetTotalCountAsync();
            var totalAnswers = await _answerRepository.GetTotalCountAsync();
            var newUsersThisWeek = await _userRepository.GetNewUsersCountAsync(7);
            var newQuestionsThisWeek = await _questionRepository.GetNewQuestionsCountAsync(7);
            var newAnswersThisWeek = await _answerRepository.GetNewAnswersCountAsync(7);

            return new AdminStats
            {
                TotalUsers = totalUsers,
                TotalQuestions = totolQuestions,
                TotalAnswers = totalAnswers,
                NewUsersThisWeek = newUsersThisWeek,
                NewQuestionsThisWeek = newQuestionsThisWeek,
                NewAnswersThisWeek = newAnswersThisWeek
            };

        }

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

        //Business logic: ban or unban
        public async Task<User?> SetUserBanStatusAsync(int userId, bool isBanned)
        {
            var updatedUser = await _userRepository.UpdateBanStatusAsync(userId, isBanned);

            if (updatedUser == null)
            {
                return null;
            }

            return updatedUser;

        }

    }
}
