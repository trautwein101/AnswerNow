using AnswerNow.Domain.Enums;

namespace AnswerNow.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsProfessional { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsInActive { get; set; } = false;
        public bool IsPending { get; set; } = false;
        public bool IsSuspended { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public DateTime LastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

        // Question flags (split by role in the workflow)
        public ICollection<QuestionFlagEntity> OwnedQuestionFlag { get; set; } = new List<QuestionFlagEntity>();
        public ICollection<QuestionFlagEntity> ReportedQuestionFlag { get; set; } = new List<QuestionFlagEntity>();
        public ICollection<QuestionFlagEntity> ResolvedQuestionFlag { get; set; } = new List<QuestionFlagEntity>();
        public ICollection<QuestionFlagEntity> DeletedQuestionFlag { get; set; } = new List<QuestionFlagEntity>();

        // Answer flags (split by role in the workflow)
        public ICollection<AnswerFlagEntity> OwnedAnswerFlag { get; set; } = new List<AnswerFlagEntity>();
        public ICollection<AnswerFlagEntity> ReportedAnswerFlag { get; set; } = new List<AnswerFlagEntity>();
        public ICollection<AnswerFlagEntity> ResolvedAnswerFlag { get; set; } = new List<AnswerFlagEntity>();
        public ICollection<AnswerFlagEntity> DeletedAnswerFlag { get; set; } = new List<AnswerFlagEntity>();

        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();

    }
}
