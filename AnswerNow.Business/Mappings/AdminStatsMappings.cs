
using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class AdminStatsMappings
    {

        public static AdminStatsDto ToDto(this AdminStats entity)
        {
            return new AdminStatsDto
            {
                TotalUsers = entity.TotalUsers,
                TotalQuestions = entity.TotalQuestions,
                TotalAnswers = entity.TotalAnswers,
                NewAnswersThisWeek = entity.NewAnswersThisWeek,
                NewQuestionsThisWeek = entity.NewQuestionsThisWeek,
                NewUsersThisWeek = entity.NewUsersThisWeek
            };
        }

    }
}
