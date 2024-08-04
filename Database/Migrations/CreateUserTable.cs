using Game.Database.Core;

namespace Game.Database.Migrations
{
    public class CreateUsersTable : DatabaseMigration
    {
        protected override void Run(IDatabaseConnection databaseConnection)
        {
            var query = DatabaseTableBuilder.Table("users")
                .AddColumn("id", "TEXT PRIMARY KEY")
                .AddColumn("login", "TEXT NOT NULL UNIQUE")
                .AddColumn("password", "TEXT NOT NULL");

            databaseConnection.Execute(query);
        }
    }
}