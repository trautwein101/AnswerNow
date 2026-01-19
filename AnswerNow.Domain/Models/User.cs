using AnswerNow.Domain.Enums;

namespace AnswerNow.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public UserRole Role { get; set; } = UserRole.User;
        public bool IsProfessional { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;
        public bool IsSuspended { get; set; } = false;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
        public int QuestionCount { get; set; } = 0;
        public int AnswerCount { get; set; } = 0;
        public int QuestionFlagCount { get; set; } = 0;
        public int AnswerFlagCount { get; set; } = 0;

    }
}
