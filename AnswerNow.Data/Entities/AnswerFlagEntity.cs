
using AnswerNow.Domain.Enums;

namespace AnswerNow.Data.Entities
{
    public class AnswerFlagEntity
    {
        public int Id { get; set; }
        public int AnswerId { get; set; }
        public int AnswerOwnerUserId { get; set; }
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

        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public DateTime? DateDeleted { get; set; }


        // NAVIGATION PROPERTIES
        public AnswerEntity Answer { get; set; } = null!;
        public UserEntity AnswerOwnerUser { get; set; } = null!;
        public UserEntity ReportedByUser { get; set; } = null!;
        public UserEntity? ResolvedByUser { get; set; }
        public UserEntity? DeletedByUser { get; set; }

    }
}
