using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class AnswerMappings
    {

        public static AnswerDto ToDto(this Answer entity) 
        {
            return new AnswerDto
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

        public static Answer ToEntity(this AnswerDto dto)
        {
            return new Answer
            {
                Id = dto.Id,
                QuestionId = dto.QuestionId,
                Body = dto.Body,
                UserId = dto.UserId,
                CreatedBy = dto.CreatedBy,
                UpVotes = dto.UpVotes,
                DownVotes = dto.DownVotes,
                IsFlagged = dto.IsFlagged,
                DateCreated = dto.DateCreated,
                DateUpdated = dto.DateUpdated
            };
        }

    }
}
