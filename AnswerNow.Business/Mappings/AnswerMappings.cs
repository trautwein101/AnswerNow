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
                UpVotes = entity.UpVotes,
                DownVotes = entity.DownVotes,
                IsFlagged = entity.IsFlagged,
                IsDeleted = entity.IsDeleted,
                DateDeleted = entity.DateDeleted,
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
                UpVotes = dto.UpVotes,
                DownVotes = dto.DownVotes,
                IsFlagged = dto.IsFlagged,
                IsDeleted = dto.IsDeleted,
                DateDeleted = dto.DateDeleted,
                DateCreated = dto.DateCreated,
                DateUpdated = dto.DateUpdated
            };
        }
    }
}
