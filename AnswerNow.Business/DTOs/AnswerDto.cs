using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{
    public class AnswerDto
    {

        public int Id { get; set; }

        [Required]
        public int QuestionId { get; set; }

        [Required]
        public string Body { get; set; } = "";

        [Required]
        public string CreatedBy { get; set; } = "";

        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

    }
}
