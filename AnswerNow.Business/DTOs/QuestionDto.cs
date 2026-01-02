using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{
    public class QuestionDto
    {

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = "";

        [Required]
        public string Body { get; set; } = "";

        // Optional - null for anonymous questions, set when user is logged in
        public int? UserId { get; set; }

        [Required]
        public string CreatedBy { get; set; } = "";

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }


    }
}
