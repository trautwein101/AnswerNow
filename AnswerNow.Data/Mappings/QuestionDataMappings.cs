using AnswerNow.Data.Entities;
using AnswerNow.Domain.Models;

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
                IsFlagged = entity.IsFlagged,

                IsDeleted = entity.IsDeleted,
                DeletedByUserId = entity.DeletedByUserId,
                DateDeleted = entity.DateDeleted,

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
