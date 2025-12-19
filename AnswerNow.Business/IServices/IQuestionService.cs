using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(int id);
        Task<Question> CreateAsync(Question question);

    }
}
