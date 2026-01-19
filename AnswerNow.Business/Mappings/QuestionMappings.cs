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
                CreatedBy = entity.CreatedBy,
                IsFlagged = entity.IsFlagged,
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
                CreatedBy = dto.CreatedBy,
                IsFlagged = dto.IsFlagged,
                DateCreated = dto.DateCreated,
                DateUpdated = dto.DateUpdated
            };
        }

    }
}
