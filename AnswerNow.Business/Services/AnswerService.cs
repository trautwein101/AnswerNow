using AnswerNow.Business.DTOs;
using AnswerNow.Business.IServices;
using AnswerNow.Business.Mappings;
using AnswerNow.Data.IRepositories;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Services
{
    public class AnswerService : IAnswerService
    {

        private readonly IAnswerRepository _answerRepository;
        public AnswerService(IAnswerRepository answerRepository)
        {
            _answerRepository = answerRepository;
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _answerRepository.GetByIdAsync(id);        
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            return await _answerRepository.GetByQuestionIdAsync(questionId);
        }

        public async Task<IEnumerable<Answer>> GetAllAsync()
        {
            return await _answerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<AnswerDto>> GetAllDtosAsync()
        {
            var entities = await _answerRepository.GetAllWithUsersAsync();

            return entities.Select(a => a.ToDto());
        }

        public async Task<AnswerDto?> GetByIdDtoAsync(int id)
        {
            var entity = await _answerRepository.GetByIdWithUserAsync(id);
            return entity?.ToDto();
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            {
                return await _answerRepository.CreateAsync(answer);
            }
        }

        public async Task<Answer?> VoteAsync(int AnswerId, bool isUpVote)
        {
            var answer = await _answerRepository.GetByIdAsync(AnswerId);

            if (answer == null)
            {
                return null;
            }

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

    }
}
