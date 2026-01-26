
namespace AnswerNow.Data.Entities
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public int UserId { get; set; }

        public bool IsFlagged { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }

        // NAVIGATION PROPERTIES
        public UserEntity User { get; set; } = null!;
        public UserEntity? DeletedByUser { get; set; }
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
        public ICollection<QuestionFlagEntity> QuestionFlag { get; set; } = new List<QuestionFlagEntity>();

    }
}
