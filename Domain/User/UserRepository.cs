using Game.Database;
using Game.Database.Migrations;

namespace Game.Domain.User;

public class UserRepository : Repository
{
    public override void Init()
    {
        Console.WriteLine("UserRepository: init");
        DatabaseService.Instance.AddMigration<CreateUsersTable>();
    }
}