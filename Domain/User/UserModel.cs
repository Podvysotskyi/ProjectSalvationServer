using System.Data;
using Game.Database.Core;

namespace Game.Domain.User;

public class UserModel : Model
{
    public static string Table = "";
    
    public readonly string Login;
    public readonly string Password;

    public UserModel(IDataReader reader)
    {
        Id = reader.GetString(0);
        Login = reader.GetString(1);
        Password = reader.GetString(2);
    }
    
    public UserModel(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public static DatabaseQuery Query()
    {
        return DatabaseQueryBuilder.Table("users");
    }
    
    public DatabaseQuery Create()
    {
        return DatabaseQueryBuilder.Insert("users")
            .Data("id", Id)
            .Data("login", Login)
            .Data("password", Password);
    }
}