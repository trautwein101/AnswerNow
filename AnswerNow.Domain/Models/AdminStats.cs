
namespace AnswerNow.Domain.Models
{
    public class AdminStats
    {
        public int TotalUsers { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalAnswers { get; set; }
        public int NewUsersThisWeek { get; set; }
        public int NewQuestionsThisWeek { get; set; }
        public int NewAnswersThisWeek { get; set; }

    }
}
