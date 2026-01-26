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

        // Set server-side from authenticated user (not required from client)
        public int UserId { get; set; }

        public string? CreatedByEmail { get; set; }
        public string? CreatedByDisplayName { get; set; }

        public int UpVotes { get; set; }
        public int DownVotes { get; set; }

        public bool IsFlagged { get; set; } = false;
        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
