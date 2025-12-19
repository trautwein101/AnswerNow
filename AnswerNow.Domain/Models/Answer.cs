
namespace AnswerNow.Domain.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Body { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

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
