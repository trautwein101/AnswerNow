using AnswerNow.Business.DTOs;
using AnswerNow.Data.Entities;

namespace AnswerNow.Business.Mappings
{
    public static class AnswerEntityMappings
    {
        public static AnswerDto ToDto(this AnswerEntity a)
        {
            return new AnswerDto
            {
                Id = a.Id,
                QuestionId = a.QuestionId,
                Body = a.Body,
                UserId = a.UserId,

                CreatedByEmail = a.User?.Email,
                CreatedByDisplayName = a.User?.DisplayName,

                UpVotes = a.UpVotes,
                DownVotes = a.DownVotes,

                IsFlagged = a.IsFlagged,
                IsDeleted = a.IsDeleted,
                DateDeleted = a.DateDeleted,

                DateCreated = a.DateCreated,
                DateUpdated = a.DateUpdated
            };
        }



    }
}
