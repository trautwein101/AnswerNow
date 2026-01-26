
namespace AnswerNow.Data.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Body { get; set; } = "";
        public int UserId { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool IsFlagged { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        // NAVIGATION PROPERTIES
        public UserEntity User { get; set; } = null!;
        public UserEntity? DeletedByUser { get; set; }
        public QuestionEntity Question { get; set; } = null!;
        public ICollection<AnswerFlagEntity> AnswerFlag { get; set; } = new List<AnswerFlagEntity>();

    }
}
