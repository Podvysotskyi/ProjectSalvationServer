using Game.Helpers;

namespace Game.Domain.User;

public class UserModel : Model
{
    public readonly string Login;
    public readonly string Password;

    public UserModel(string login, string password): base()
    {
        Login = login;
        Password = password;
    }

    public UserModel(string id, string login, string password) : base(id)
    {
        Login = login;
        Password = password;
    }
}