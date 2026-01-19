using AnswerNow.Business.IServices;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Services
{
    public class ModeratorService : IModeratorService
    {

        private readonly IAnswerRepository _answerRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;

        public ModeratorService(IAnswerRepository answerRepository, IQuestionRepository questionRepository, IUserRepository userRepository)
        {
            _answerRepository = answerRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
        }

        public async Task<ModeratorStats> GetModeratorStatsAsync()
        {

            var totalIsFlaggedQuestions = await _questionRepository.GetTotalIsFlaggedCountAsync();
            var totalIsFlaggedAnswers = await _answerRepository.GetTotalIsFlaggedCountAsync();
            var newIsFlaggedQuestionsThisWeek = await _questionRepository.GetTotalIsFlaggedCountAsync();
            var newIsFlaggedAnswersThisWeek = await _questionRepository.GetTotalIsFlaggedCountAsync();

            return new ModeratorStats
            {
                TotalIsFlaggedQuestions = totalIsFlaggedQuestions,
                TotalIsFlaggedAnswers = totalIsFlaggedAnswers,
                NewIsFlaggedQuestionsThisWeek = newIsFlaggedQuestionsThisWeek,
                NewIsFlaggedAnswersThisWeek = newIsFlaggedAnswersThisWeek
            };

        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        //Business logic: suspend and unsuspend
        public async Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended)
        {
            var updatedUser = await _userRepository.UpdateSuspendStatusAsync(userId, isSuspended);

            if (updatedUser == null)
            {
                return null;
            }

            return updatedUser;

        }

    }
}
