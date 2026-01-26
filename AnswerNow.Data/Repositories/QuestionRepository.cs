using AnswerNow.Data.Entities;
using AnswerNow.Data.IRepositories;
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
                .AsNoTracking()
                .OrderByDescending(q => q.DateCreated)
                .ToListAsync();

            return entities.Select(e => e.ToDomain());
        }

        public async Task<List<QuestionEntity>> GetAllWithUsersAsync()
        {
            return await _dbContext.Questions
                .AsNoTracking()
                .Include(q => q.User)
                .OrderByDescending(q => q.DateCreated)
                .ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Questions.FindAsync(id);
            return entity?.ToDomain();
        }

        public async Task<QuestionEntity?> GetByIdWithUserAsync(int id)
        {
            return await _dbContext.Questions
                .AsNoTracking()
                .Include(q => q.User)
                .SingleOrDefaultAsync(q => q.Id == id);
        }

        public async Task<Question> CreateAsync(Question question)
        {
            var entity = question.ToEntity();
            entity.DateCreated = DateTime.UtcNow;
            entity.DateUpdated = DateTime.UtcNow;

            _dbContext.Questions.Add(entity);
            await _dbContext.SaveChangesAsync();

            return entity.ToDomain();

        }

        //Admin methods
        public async Task<int> GetTotalCountAsync()
        {
            return await _dbContext.Questions.CountAsync();
        }

        public async Task<int> GetNewQuestionsCountAsync(int days)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Questions.CountAsync(q => q.DateCreated >= cutOffDate);
        }

        //Moderator methods
        public async Task<int> GetTotalIsFlaggedCountAsync()
        {
            return await _dbContext.Questions.CountAsync(q => q.IsFlagged == true);
        }

        public async Task<int> GetNewIsFlaggedCountAsync(int days)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Questions.CountAsync(q => q.IsFlagged && q.DateCreated >= cutOffDate);
        }

    }
}
