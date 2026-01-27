using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IAnswerService
    {
       Task<IEnumerable<Answer>> GetAllAsync();
       Task<IEnumerable<AnswerDto>> GetAllDtosAsync();
       Task<Answer?> GetByIdAsync(int id);
       Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
       Task<Answer> CreateAsync(Answer answer);
       Task<Answer?> VoteAsync(int answerId, bool isUpVote);

    }
}
