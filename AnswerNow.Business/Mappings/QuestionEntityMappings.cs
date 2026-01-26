using AnswerNow.Business.DTOs;
using AnswerNow.Data.Entities;

namespace AnswerNow.Business.Mappings
{
    public static class QuestionEntityMappings
    {
        public static QuestionDto ToDto(this QuestionEntity q)
        {
            return new QuestionDto
            {
                Id = q.Id,
                Title = q.Title,
                Body = q.Body,
                UserId = q.UserId,

                CreatedByEmail = q.User?.Email,
                CreatedByDisplayName = q.User?.DisplayName,

                IsFlagged = q.IsFlagged,
                IsDeleted = q.IsDeleted,
                DateDeleted = q.DateDeleted,

                DateCreated = q.DateCreated,
                DateUpdated = q.DateUpdated
            };

        }

    }
}
