
namespace AnswerNow.Business.DTOs
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public DateTime DateCreated { get; set; }
        public bool IsBanned { get; set; }
        public bool IsActive { get; set; }
        public int QuestionCount { get; set; }
        public int AnswerCount {  get; set; }

    }
}
