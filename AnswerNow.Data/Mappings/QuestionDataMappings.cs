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
                DateCreated = entity.DateCreated,
                UserId = entity.UserId,
                CreatedBy = entity.CreatedBy
            };
        }

        public static QuestionEntity ToEntity(this Question domain)
        {
            return new QuestionEntity
            {
                Id = domain.Id,
                Title = domain.Title,
                Body = domain.Body,
                DateCreated = domain.DateCreated,
                UserId = domain.UserId,
                CreatedBy = domain.CreatedBy
            };
        }

    }
}
