
namespace AnswerNow.Business.DTOs
{
    public class ChangeRoleDto
    {

        //Single Responsibilty for assigning new role to user, moderator, and admin
        public string NewRole { get; set; } = string.Empty;

    }
}
