
namespace AnswerNow.Data.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Body { get; set; } = "";
        public int? UserId { get; set; }
        public string CreatedBy { get; set; } = "";
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool IsFlagged { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        // NAVIGATION PROPERTIES
        public QuestionEntity Question { get; set; } = null!;
        public UserEntity? User { get; set; }

    }
}
