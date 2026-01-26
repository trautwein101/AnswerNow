
namespace AnswerNow.Domain.Models
{
    public class Question
    {

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public int UserId { get; set; }
        public bool IsFlagged { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }
        public DateTime? DateDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }


    }
}
