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

        /// <inheritdoc />
        public async Task<ModeratorStats> GetModeratorStatsAsync()
        {
            
            // todo: run in parallel for optimization
            var totalIsFlaggedQuestions = await _questionRepository.GetTotalIsFlaggedCountAsync();
            var totalIsFlaggedAnswers = await _answerRepository.GetTotalIsFlaggedCountAsync();
            var newIsFlaggedQuestionsThisWeek = await _questionRepository.GetNewIsFlaggedCountAsync(7);
            var newIsFlaggedAnswersThisWeek = await _answerRepository.GetNewIsFlaggedCountAsync(7);

            return new ModeratorStats
            {
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


        /// <inheritdoc />
        public async Task<User?> SetUserSuspendStatusAsync(int userId, bool isSuspended)
        {

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return null;

            var newStatus = GetNewStatus(user, UserStatus.Suspended, isSuspended);
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
