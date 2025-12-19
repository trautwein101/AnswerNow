using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IAnswerService
    {

       Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
       Task<Answer?> GetByIdAsync(int id);
       Task<Answer> CreateAsync(Answer answer);
       Task<Answer?> VoteAsync(int answerId, bool isUpVote);

    }
}
