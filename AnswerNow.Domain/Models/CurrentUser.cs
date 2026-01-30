using AnswerNow.Domain.Enums;

namespace AnswerNow.Domain.Models
{
    public class CurrentUser
    {
        public bool IsAuthenticated { get; init; }
        public int? UserId { get; init; }
        public UserRole? Role { get; init; }

        public bool IsInRole(UserRole role) => Role == role;

        public bool IsAtLeast(UserRole role) =>
            Role.HasValue && Role.Value >= role;

    }
}
