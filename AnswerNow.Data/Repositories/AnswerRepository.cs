using AnswerNow.Data.Entities;
using AnswerNow.Data.IRepositories;
using AnswerNow.Data.Mappings;
using AnswerNow.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AnswerNow.Data.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AnswerNowDbContext _dbContext;

        public AnswerRepository(AnswerNowDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Answer>> GetByQuestionIdAsync(int questionId)
        {
            var entities = await _dbContext.Answers
                    .Where(a => a.QuestionId == questionId)
                    .OrderByDescending(a => a.UpVotes - a.DownVotes)
                    .ThenByDescending(a => a.DateCreated)
                    .ToListAsync();

            return entities.Select(e => e.ToDomain());
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            var entity = await _dbContext.Answers.FindAsync(id);

            return entity?.ToDomain();
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            // Convert Domain Model → Entity
            var entity = answer.ToEntity();
            entity.DateCreated = DateTime.Now;
            entity.UpVotes = 0;
            entity.DownVotes = 0;

            _dbContext.Answers.Add(entity);
            await _dbContext.SaveChangesAsync();

            //Return Domain Model
            return entity.ToDomain();
        }

        public async Task<Answer> UpdateAsync(Answer answer)
        {
            var entity = _dbContext.Answers.Find(answer.Id);

            if (entity == null)
            {
                throw new InvalidOperationException($"Answer {answer.Id} not found");
            }

            entity.Body = answer.Body;
            entity.UpVotes = answer.UpVotes;
            entity.DownVotes = answer.DownVotes;
            entity.DateUpdated = DateTime.Now;

            await _dbContext.SaveChangesAsync();
            return entity.ToDomain();
        }


        //Admin methods
        public async Task<int> GetTotalCountAsync()
        {
            return await _dbContext.Answers.CountAsync();
        }

        public async Task<int> GetNewAnswersCountAsync(int days)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-days);
            
            return await _dbContext.Answers.CountAsync(a => a.DateCreated >= cutOffDate);
        }


        //Moderator methods
        public async Task<int> GetTotalIsFlaggedCountAsync()
        {
            return await _dbContext.Answers.CountAsync(a => a.IsFlagged == true);
        }

        public async Task<int> GetNewIsFlaggedCountAsync(int days)
        {
            var cutOffDate = DateTime.UtcNow.AddDays(-days);

            return await _dbContext.Answers.CountAsync(a => a.DateCreated >= cutOffDate);
        }


    }
}
