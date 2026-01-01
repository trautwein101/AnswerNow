
namespace AnswerNow.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } = "";
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        public string? DeviceInfo { get; set; }

        //Helper Property
        public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;

        //Associated User ~ optional and loaded when needed
        public User? User { get; set; }

    }
}
