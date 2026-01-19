
namespace AnswerNow.Domain.Enums
{
    public enum UserRole
    {
        User = 0, //default role
        Professional = 1, //professional role ~ business user
        Moderator = 2, //all user access, edit/delete content, warn users
        Admin = 3, //full access to manage users and assing roles
        AI = 4 //AI oriented services
    }
}
