using Game.Database;
using Game.Database.Migrations;
using Game.Helpers;

namespace Game.Domain.User;

public class UserRepository : Repository
{
    public override void Init()
    {
        Console.WriteLine("UserRepository: init");
        DatabaseService.Instance.AddMigration<CreateUsersTable>();
    }

    public List<UserModel> Get(string login)
    {
        using var connection = DatabaseService.OpenConnection();
        var query = UserModel.Query().Where("login", login);
        
        var result = new List<UserModel>();
        
        var reader = connection.Read(query);
        while (reader.Read())
        {
            result.Add(new UserModel(reader));
        }
        
        return result;
    }

    public UserModel? Find(string id)
    {
        using var connection = DatabaseService.OpenConnection();
        var query = UserModel.Query().Where("id", id).Limit(1);

        var reader = connection.Read(query);
        while (reader.Read())
        {
            return new UserModel(reader);
        }

        return null;
    }
    
    public UserModel? Create(string login, string password)
    {
        var passwordHash = HashHelper.GetSha256Hash(password);
        
        using var connection = DatabaseService.OpenConnection();

        var user = new UserModel(login, passwordHash);

        connection.Execute(user.Create());
        
        return Find(user.Id);
    }

    public bool Exists(string login)
    {
        using var connection = DatabaseService.OpenConnection();
        var query = UserModel.Query().Where("login", login).Exists();
        return connection.Exists(query);
    }
}