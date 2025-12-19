using AnswerNow.Domain.Enums;

namespace AnswerNow.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public DateTime DateCreated { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsActive { get; set; } = true;
        public UserRole Role { get; set; } = UserRole.User;

        // NAVIGATION PROPERTY
        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
        public ICollection<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();

    }
}
