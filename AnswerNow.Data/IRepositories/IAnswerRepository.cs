using AnswerNow.Domain.Models;

namespace AnswerNow.Data.IRepositories
{
    // The Repository Pattern abstracts data access.
    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);
        Task<Answer?> GetByIdAsync(int id);
        Task<Answer> CreateAsync(Answer answer);
        Task<Answer> UpdateAsync(Answer answer);

        //Admin Methods
        Task<int> GetTotalCountAsync();
        Task<int> GetNewAnswersCountAsync(int days);

    }
}
