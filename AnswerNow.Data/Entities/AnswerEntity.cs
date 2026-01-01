
namespace AnswerNow.Data.Entities
{
    public class AnswerEntity
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Body { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // NAVIGATION PROPERTY
        public QuestionEntity? Question { get; set; }

    }
}
