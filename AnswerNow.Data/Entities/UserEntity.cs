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
        public bool IsBanned { get; set; } = false;
        public bool IsSuspended { get; set; } = false;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;

        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
        public ICollection<RefreshTokenEntity> RefreshTokens { get; set; } = new List<RefreshTokenEntity>();

    }
}
