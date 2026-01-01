
namespace AnswerNow.Business.DTOs
{
    public class AdminStatsDto
    {

        public int TotalUsers { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalAnswers {  get; set; }
        public int NewUsersThisWeeks { get; set; }
        public int NewQuestionsThisWeeks { get;set; }

    }
}
