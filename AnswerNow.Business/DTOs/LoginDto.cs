using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{
    public class LoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

    }
}
