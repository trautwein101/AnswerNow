
namespace AnswerNow.Business.DTOs
{
    public class UserDto
    {

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Role { get; set; } = "User";
        public bool IsProfessional { get; set; }
        public bool IsActive { get; set; }
        public bool IsInActive { get; set; }
        public bool IsPending { get; set; }
        public bool IsSuspended { get; set; }
        public bool IsBanned { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int QuestionCount { get; set; }
        public int AnswerCount {  get; set; }
        public int QuestionFlagCount { get; set; }
        public int AnswerFlagCount { get; set; }

    }
}
