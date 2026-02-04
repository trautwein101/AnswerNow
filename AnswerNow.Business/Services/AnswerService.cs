using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Enums;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Services
{
    public class AnswerService : IAnswerService
    {

        private readonly IAnswerRepository _answerRepository;
        private readonly ICurrentUserService _currentUserService;
        public AnswerService(IAnswerRepository answerRepository, ICurrentUserService currentUserService)
        {
            _answerRepository = answerRepository;
            _currentUserService = currentUserService;
        }

        /// <inheritdoc />
        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _answerRepository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            return await _answerRepository.GetByQuestionIdAsync(questionId);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _answerRepository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<AnswerDto>> GetAllDtosAsync()
        {
            var entities = await _answerRepository.GetAllWithUsersAsync();

            return entities.Select(a => a.ToDto());
        }

        /// <inheritdoc />
        public async Task<AnswerDto?> GetByIdDtoAsync(int id)
        {
            var entity = await _answerRepository.GetByIdWithUserAsync(id);
            return entity?.ToDto();
        }

        /// <inheritdoc />
        public async Task<Answer> CreateAsync(Answer answer)
        {
            {
                return await _answerRepository.CreateAsync(answer);
            }
        }

        /// <inheritdoc />
        public async Task<Answer?> VoteAsync(int answerId, bool isUpVote)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null) return null;

            if (isUpVote)
            {
                answer.Upvote();
            }
            else
            {
                answer.DownVote();
            }

            return await _answerRepository.UpdateAsync(answer);

        }

        /// <inheritdoc />
        public async Task<Answer?> FlaggedAsync(int answerId, bool isFlagged)
        {
            var answer = await _answerRepository.GetByIdAsync(answerId);
            if (answer == null) return null;

            var currentUser = _currentUserService.Get();

            // Admin/Moderator: full control
            if (currentUser.IsAtLeast(UserRole.Moderator))
            {
                if (answer.IsFlagged == isFlagged)
                    return answer; // no change

                answer.IsFlagged = isFlagged;
                return await _answerRepository.UpdateAsync(answer);
            }

            // Normal user: only allow false -> true
            if (!answer.IsFlagged && isFlagged)
            {
                answer.IsFlagged = true;
                return await _answerRepository.UpdateAsync(answer);
            }

            //no change needed or allowed
            return answer;

        }

    }
}
