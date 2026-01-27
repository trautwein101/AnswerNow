using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.IRepositories
{

    public interface IAnswerRepository
    {
        Task<IEnumerable<Answer>> GetAllAsync();
        Task<List<AnswerEntity>> GetAllWithUsersAsync();
        Task<Answer?> GetByIdAsync(int id);
        Task<AnswerEntity?> GetByIdWithUserAsync(int id);
        Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId);

        Task<Answer> UpdateAsync(Answer answer);
        Task<Answer> CreateAsync(Answer answer);

        //Admin & Moderator Methods
        Task<int> GetTotalCountAsync();
        Task<int> GetNewAnswersCountAsync(int days);
        Task<int> GetTotalIsFlaggedCountAsync();
        Task<int> GetNewIsFlaggedCountAsync(int days);

    }
}
