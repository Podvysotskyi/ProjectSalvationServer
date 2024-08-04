using Game.Database.Migrations;

namespace Game.Database.Domain.Repositories
{
    public class UserRepository : Repository
    {
        public override void Init()
        {
            Console.WriteLine("UserRepository: init");
            DatabaseService.Instance.AddMigration<CreateUsersTable>();
        }
    }
}
