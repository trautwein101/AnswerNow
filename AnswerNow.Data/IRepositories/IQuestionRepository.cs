using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.IRepositories
{
    public interface IQuestionRepository
    {
        Task<IEnumerable<Question>> GetAllAsync();
        Task<List<QuestionEntity>> GetAllWithUsersAsync();
        Task<Question?> GetByIdAsync(int id);
        Task<QuestionEntity?> GetByIdWithUserAsync(int id);

        Task<Question> CreateAsync(Question question);

        //Admin & Moderator Methods
        Task<int> GetTotalCountAsync();
        Task<int> GetNewQuestionsCountAsync(int days);
        Task<int> GetTotalIsFlaggedCountAsync();
        Task<int> GetNewIsFlaggedCountAsync(int days);

    }
}
