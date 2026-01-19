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
                CreatedBy = entity.CreatedBy,
                UpVotes = entity.UpVotes,
                DownVotes = entity.DownVotes,
                IsFlagged = entity.IsFlagged,
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
                CreatedBy = domain.CreatedBy,
                UpVotes = domain.UpVotes,
                DownVotes = domain.DownVotes,
                IsFlagged = domain.IsFlagged,
                DateCreated = domain.DateCreated,
                DateUpdated = domain.DateUpdated
            };

        }



    }
}
