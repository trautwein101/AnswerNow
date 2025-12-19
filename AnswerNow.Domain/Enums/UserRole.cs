
namespace AnswerNow.Domain.Enums
{
    public enum UserRole
    {
        User = 0, //default role
        Moderator = 1, //all user access, edit/delete content, warn users
        Admin = 2 //full access to manage users and assing roles
    }
}
