
namespace AnswerNow.Data.Entities
{
    public class RefreshTokenEntity
    {

        public int Id { get; set; }
        public string Token { get; set; } = "";
        public int UserId { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        public string? DeviceInfo { get; set; }
        public UserEntity User { get; set; } = null!;
        public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;

    }
}
