
namespace AnswerNow.Domain.Models
{
    public class Question
    {

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

    }
}
