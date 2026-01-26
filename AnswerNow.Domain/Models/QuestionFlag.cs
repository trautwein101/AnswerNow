
using AnswerNow.Domain.Enums;

namespace AnswerNow.Domain.Models
{
    public class QuestionFlag
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public int QuestionOwnerUserId { get; set; }
        public int ReportedByUserId { get; set; }
        public FlagReason Reason { get; set; } //enum of why
        public string? Notes { get; set; } //free text

        //Resolution Workflow
        public bool IsResolved { get; set; } = false;
        public int? ResolvedByUserId { get; set; }
        public DateTime? DateResolved { get; set; }
        public string? ResolutionNotes { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? DeletedByUserId { get; set; }
        public DateTime? DateDeleted { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

    }
}
