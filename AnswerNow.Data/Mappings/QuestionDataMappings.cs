using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;
using System.Net.NetworkInformation;

namespace AnswerNow.Data.Mappings
{
    public static class QuestionDataMappings
    {
        public static Question ToDomain(this QuestionEntity entity)
        {
            return new Question
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                UserId = entity.UserId,
                CreatedBy = entity.CreatedBy,
                IsFlagged = entity.IsFlagged,
                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated
            };
        }

        public static QuestionEntity ToEntity(this Question domain)
        {
            return new QuestionEntity
            {
                Id = domain.Id,
                Title = domain.Title,
                Body = domain.Body,
                UserId = domain.UserId,
                CreatedBy = domain.CreatedBy,
                IsFlagged = domain.IsFlagged,
                DateCreated = domain.DateCreated,
                DateUpdated= domain.DateUpdated
            };
        }

    }
}
