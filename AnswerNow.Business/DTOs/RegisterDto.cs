using System.ComponentModel.DataAnnotations;

namespace AnswerNow.Business.DTOs
{

    // Received from Angular with new sign up.
    public class RegisterDto
    {

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100)]
        public string DisplayName { get; set; } = "";

        [Required]
        [StringLength(100, MinimumLength = 6)] // min 6 characters
        public string Password { get; set; } = "";

    }
}
