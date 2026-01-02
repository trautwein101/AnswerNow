
namespace AnswerNow.Domain.Models
{
    public class AuthResponse
    {
        public string AccessToken { get; set; } = "";
        public DateTime AccessTokenExpiration { get; set; }
        public string RefreshToken { get; set; } = "";
        public DateTime RefreshTokenExpiration { get; set; }

        public User? User { get; set; }


    }
}
