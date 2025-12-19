using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{

    // Response to Angular after successful login/register for storage ~includes active token (short lived) refresh-token(long lived)
    public class AuthResponseDto
    {

        //Access Token short live to 15 min and sent with every API request.
        public string AccessToken { get; set; } = "";
        public DateTime AccessTokenExpiration { get; set; }

        //Access Token long lived to 7 days, used to get new access tokens
        public string RefreshToken { get; set; } = "";
        public DateTime RefreshTokenExpiration {  get; set; }

        //User info for UI display
        public int UserId { get; set; }
        public string Email { get; set; } = "";
        public string DisplayName { get; set; } = "";

        //Security Roles
        public string Role { get; set; } = "User";

    }
}
