namespace Game.Database.Core
{
    public abstract class DatabaseMigration
    {
        public bool Execute(DatabaseConnection connection)
        {
            var name = GetType().ToString();
            
            if (Exists(connection, name))
            {
                return true;
            }
            
            using (var transaction = connection.StartTransaction())
            {
                try
                {
                    Run(transaction);
                    Save(transaction, name);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                    transaction.Rollback();
                    return false;
                }
            }

            return true;
        }

        protected abstract void Run(IDatabaseConnection databaseConnection);
        
        public static void CreateMigrationTable(IDatabaseConnection connection)
        {
            var query = DatabaseTableBuilder.Table("migrations")
                .AddColumn("id", "INTEGER PRIMARY KEY")
                .AddColumn("migration", "TEXT NOT NULL");
            
            connection.Execute(query);
        }

        private static bool Exists(IDatabaseConnection connection, string name)
        {
            var query = DatabaseQueryBuilder.Table("migrations")
                .Where("migration", name);

            return connection.Exists(query);
        }

        private static void Save(IDatabaseConnection connection, string name)
        {
            var query = DatabaseQueryBuilder.Insert("migrations")
                .Data("migration", name);

            connection.Execute(query);
        }
    }
}
