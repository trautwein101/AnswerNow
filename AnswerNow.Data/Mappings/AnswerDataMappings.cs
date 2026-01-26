using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.Mappings
{
    public static class AnswerDataMappings
    {
        public static Answer ToDomain(this AnswerEntity entity)
        {
            return new Answer
            {
                Id = entity.Id,
                QuestionId = entity.QuestionId,
                Body = entity.Body,
                UserId = entity.UserId,
                UpVotes = entity.UpVotes,
                DownVotes = entity.DownVotes,
                IsFlagged = entity.IsFlagged,
                IsDeleted = entity.IsDeleted,
                DeletedByUserId = entity.DeletedByUserId,
                DateDeleted = entity.DateDeleted,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated
            };
        }

        public static AnswerEntity ToEntity(this Answer domain)
        {
            return new AnswerEntity
            {
                Id = domain.Id,
                QuestionId = domain.QuestionId,
                Body = domain.Body,
                UserId = domain.UserId,
                UpVotes = domain.UpVotes,
                DownVotes = domain.DownVotes,
                IsFlagged = domain.IsFlagged,
                IsDeleted = domain.IsDeleted,
                DeletedByUserId = domain.DeletedByUserId,
                DateDeleted = domain.DateDeleted,
                DateCreated = domain.DateCreated,
                DateUpdated = domain.DateUpdated
            };
        }
    }
}
