using AnswerNow.Business.DTOs;
using AnswerNow.Domain.Models;

namespace AnswerNow.Business.Mappings
{
    public static class ModeratorStatsMappings
    {

        public static ModeratorStatsDto ToDto(this ModeratorStats entity)
        {
            return new ModeratorStatsDto
            {
                TotalIsFlaggedQuestions = entity.TotalIsFlaggedQuestions,
                TotalIsFlaggedAnswers = entity.TotalIsFlaggedAnswers,
                NewIsFlaggedQuestionsThisWeek = entity.NewIsFlaggedQuestionsThisWeek,
                NewIsFlaggedAnswersThisWeek = entity.NewIsFlaggedAnswersThisWeek
                
            };
        }

    }
}



