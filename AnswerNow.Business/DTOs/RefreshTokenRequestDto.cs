using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{
    public class RefreshTokenRequestDto
    {

        [Required]
        public string RefreshToken { get; set; } = "";
    
    }
}
