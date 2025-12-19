using AnswerNow.Data.Entities;
using AnswerNow.Data.Mappings;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AnswerNow.Data.Repositories
{
    public class QuestionRepository : IQuestionRepository 
    {
        private readonly AnswerNowDbContext _dbContext;

        public QuestionRepository(AnswerNowDbContext dbContext)
        {
                _dbContext = dbContext;
        }

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            var entities = await _dbContext.Questions
                .OrderByDescending(q => q.DateCreated)
                .ToListAsync();

            return entities.Select(e => e.ToDomain());
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Questions.FindAsync(id);
            return entity?.ToDomain();
        }

        public async Task<Question> CreateAsync(Question question)
        {
            var entity = question.ToEntity();
            entity.DateCreated = DateTime.Now;
            
            _dbContext.Questions.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.ToDomain();

        }


    }
}
