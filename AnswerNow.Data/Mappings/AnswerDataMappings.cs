using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

namespace AnswerNow.Data.Mappings
{

    // DATA LAYER MAPPINGS  
    // Converts between Domain Models and EF Entities.

    public static class AnswerDataMappings
    {

        // ENTITY → DOMAIN MODEL
        public static Answer ToDomain(this AnswerEntity entity)
        {
            return new Answer
            {
                Id = entity.Id,
                QuestionId = entity.QuestionId,
                Body = entity.Body,
                CreatedBy = entity.CreatedBy,
                UpVotes = entity.UpVotes,
                DownVotes = entity.DownVotes,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated
            };
        }

        // DOMAIN MODEL → ENTITY
        public static AnswerEntity ToEntity(this Answer domain)
        {
            return new AnswerEntity
            {
                Id = domain.Id,
                QuestionId = domain.QuestionId,
                Body = domain.Body,
                CreatedBy = domain.CreatedBy,
                UpVotes = domain.UpVotes,
                DownVotes = domain.DownVotes,
                DateCreated = domain.DateCreated,
                DateUpdated = domain.DateUpdated
            };

        }



    }
}
