using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class QuestionMappings
    {

        public static QuestionDto ToDto(this Question entity)
        {
            //extension methods
            return new QuestionDto
            {
                Id = entity.Id,
                Title = entity.Title,
                Body = entity.Body,
                DateCreated = entity.DateCreated,
                CreatedBy = entity.CreatedBy
            };
        }

        public static Question ToEntity(this QuestionDto dto)
        {
            return new Question
            {
                Id = dto.Id,
                Title = dto.Title,
                Body = dto.Body,
                DateCreated = dto.DateCreated,
                CreatedBy = dto.CreatedBy
            };
        }

    }
}
