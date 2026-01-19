
namespace AnswerNow.Data.Entities
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public int UserId { get; set; }
        public string CreatedBy { get; set; } = "";
        public bool IsFlagged { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        // NAVIGATION PROPERTIES
        public UserEntity User { get; set; } = null!;
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    }
}
