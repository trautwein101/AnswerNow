using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.IServices
{
    public interface IQuestionService
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<IEnumerable<QuestionDto>> GetAllDtosAsync();
        Task<Question?> GetByIdAsync(int id);
        Task<QuestionDto?> GetByIdDtoAsync(int id);
        Task<Question> CreateAsync(Question question);

    }
}
