
namespace AnswerNow.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Body { get; set; } = "";

        public int UserId { get; set; } // require login

        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public bool IsFlagged { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }
        public DateTime? DateDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // COMPUTED PROPERTY (Business Logic)
        public int VoteScore => UpVotes - DownVotes;

        // BUSINESS METHODS
        public void Upvote()
        {
            UpVotes++;
        }

        public void DownVote()
        {
            DownVotes++;
        }

        public bool IsPopular => VoteScore >= 10;

    }
}
