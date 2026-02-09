using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;
using Microsoft.Extensions.Logging;
using AnswerNow.Utilities.Exceptions;

namespace AnswerNow.Business.Services
{
    public class QuestionService : IQuestionService
    {

        private readonly IQuestionRepository _questionRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<QuestionService> _logger;

        public QuestionService(IQuestionRepository questionRepository, ICurrentUserService currentUserService, ILogger<QuestionService> logger)
        {
            _questionRepository = questionRepository;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _questionRepository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            return await _questionRepository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<QuestionDto>> GetAllDtosAsync()
        {
            var entities = await _questionRepository.GetAllWithUsersAsync();

            return entities.Select(q => q.ToDto());
        }

        /// <inheritdoc />
        public async Task<QuestionDto> GetByIdDtoAsync(int id)
        {
            var entity = await _questionRepository.GetByIdWithUserAsync(id);

            if (entity == null)
                throw new NotFoundAppException($"Question {id} was not found.");

            return entity.ToDto();
        }

        /// <inheritdoc />
        public async Task<Question> CreateAsync(Question question)
        {

            return await _questionRepository.CreateAsync(question);
        }

        /// <inheritdoc />
        public async Task<Question?> FlaggedAsync(int questionId, bool isFlagged)
        {
            _logger.LogInformation("Flag request. QuestionId={QuestionId}, RequestedIsFlagged={RequestedIsFlagged}",
                questionId, isFlagged);

            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null)
            {
                _logger.LogWarning("Flag request failed: question not found. QuestionId={QuestionId}",
                    questionId);

                throw new NotFoundAppException($"Question {questionId} was not found.");
            }

            var currentUser = _currentUserService.Get();

            _logger.LogInformation("Flag actor. QuestionId={QuestionId}, Role={Role}",
                questionId, currentUser.Role);

            // Admin/Moderator: full control
            if (currentUser.IsAtLeast(UserRole.Moderator))
            {
                // No-op: already in desired state
                if (question.IsFlagged == isFlagged)
                {
                    _logger.LogInformation("Flag no-op. QuestionId={QuestionId}, Role={Role}, Current={Current}, Requested={Requested}",
                        questionId, currentUser.Role, question.IsFlagged, isFlagged);

                    return question;
                }

                // State change
                question.IsFlagged = isFlagged;

                _logger.LogInformation("Flag updated by elevated role. QuestionId={QuestionId}, Role={Role}, NewIsFlagged={NewIsFlagged}",
                    questionId, currentUser.Role, isFlagged);

                return await _questionRepository.UpdateAsync(question);
            }

            // Normal user: only allow false -> true
            if (!question.IsFlagged && isFlagged)
            {
                question.IsFlagged = true;

                _logger.LogInformation("Question flagged by normal user. QuestionId={QuestionId}",
                    questionId);

                return await _questionRepository.UpdateAsync(question);
            }

            // Denied or no change allowed for normal users
            _logger.LogWarning("Flag change denied. QuestionId={QuestionId}, Role={Role}, Current={Current}, Requested={Requested}",
                questionId, currentUser.Role, question.IsFlagged, isFlagged);

            throw new ForbiddenAppException("Apologies, but you are not allowed to change the flagged state of this question.");
        }

    }
}
