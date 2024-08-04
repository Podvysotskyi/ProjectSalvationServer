using Game.Helpers;

namespace Game.Database.Domain.Models
{
    public class User : Model
    {
        public readonly string Login;
        public readonly string Password;

        public User(string login, string password): base()
        {
            Login = login;
            Password = HashHelper.GetSha256Hash(password);
        }

        public User(string id, string login, string password) : base(id)
        {
            Login = login;
            Password = password;
        }
    }
}
