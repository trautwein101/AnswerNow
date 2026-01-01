
namespace AnswerNow.Data.Entities
{
    public class QuestionEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public string CreatedBy { get; set; } = "";
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        // NAVIGATION PROPERTY
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    }
}
