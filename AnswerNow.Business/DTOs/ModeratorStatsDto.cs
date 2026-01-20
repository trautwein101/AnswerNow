
namespace AnswerNow.Business.DTOs
{
    public class ModeratorStatsDto
    {

        public int TotalIsFlaggedQuestions { get; set; }
        public int TotalIsFlaggedAnswers { get; set; }
        public int NewIsFlaggedQuestionsThisWeek { get; set; }
        public int NewIsFlaggedAnswersThisWeek { get; set; }

    }
}
