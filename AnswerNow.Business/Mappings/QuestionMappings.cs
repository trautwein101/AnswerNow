using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class QuestionMappings
    {
        public static QuestionDto ToDto(this Question entity)
        {
            return new QuestionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                UserId = entity.UserId,

                IsFlagged = entity.IsFlagged,
                IsDeleted = entity.IsDeleted,
                DateDeleted = entity.DateDeleted,

                DateCreated = entity.DateCreated,
                DateUpdated = entity.DateUpdated
            };
        }

        public static Question ToEntity(this QuestionDto dto)
        {
            return new Question
            {
                Id = dto.Id,
                Title = dto.Title,
                Body = dto.Body,
                UserId = dto.UserId,

                IsFlagged = dto.IsFlagged,
                IsDeleted = dto.IsDeleted,
                DateDeleted = dto.DateDeleted,

                DateCreated = dto.DateCreated,
                DateUpdated = dto.DateUpdated
            };
        }
    }
}
