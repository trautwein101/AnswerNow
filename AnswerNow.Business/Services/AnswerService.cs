using AnswerNow.Business.IServices;
using AnswerNow.Data.Repositories;
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

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            // Repository returns Domain Models, already sorted
            return await _answerRepository.GetByQuestionIdAsync(questionId);
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _answerRepository.GetByIdAsync(id);        
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            {
                return await _answerRepository.CreateAsync(answer);
            }
        }

        public async Task<Answer?> VoteAsync(int AnswerId, bool isUpVote)
        {
            //Get domain model
            var answer = await _answerRepository.GetByIdAsync(AnswerId);

            if (answer == null)
            {
                return null;
            }

            // BUSINESS LOGIC is in Domain Model
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
