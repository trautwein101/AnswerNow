using AnswerNow.Domain.Enums;

namespace AnswerNow.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";
        public string PasswordHash { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public bool IsBanned { get; set; } = false;

        //Role will determine the permission ~ user, moderator, admin
        public UserRole Role { get; set; } = UserRole.User;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; } = DateTime.UtcNow;

    }
}
