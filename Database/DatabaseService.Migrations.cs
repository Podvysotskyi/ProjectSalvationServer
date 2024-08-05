using Game.Database.Core;
using Game.Database.Events;

namespace Game.Database
{
    public partial class DatabaseService
    {
        private readonly List<Type> _migrations;

        public event EventHandler<DatabaseMigrationStartEvent>? DatabaseStartMigrationEvent;

        public event EventHandler<DatabaseMigrationCompleteEvent>? DatabaseMigrationCompleteEvent;

        public void AddMigration<T>() where T : DatabaseMigration
        {
            var type = typeof(T);
            if (!_migrations.Contains(type))
            {
                _migrations.Add(type);
            }
        }
        
        private void RunMigrations()
        {
            Console.WriteLine("Database: run migrations");
            
            using var connection = OpenConnection();
            DatabaseMigration.CreateMigrationTable(connection);
                
            foreach (var type in _migrations)
            {
                DatabaseStartMigrationEvent?.Invoke(this, new DatabaseMigrationStartEvent(type));
                    
                var migration = Activator.CreateInstance(type) as DatabaseMigration;
                var complete = migration?.Execute(connection) ?? false;
                    
                DatabaseMigrationCompleteEvent?.Invoke(this, new DatabaseMigrationCompleteEvent(type, complete));

                if (!complete)
                {
                    return;
                }
            }
        }

        private static void OnDatabaseMigrationStart(object? sender, DatabaseMigrationStartEvent e)
        {
            Console.WriteLine($"Database: migration '{e.Type.Name}' - Start");
        }

        private static void OnDatabaseMigrationComplete(object? sender, DatabaseMigrationCompleteEvent e)
        {
            Console.WriteLine(e.IsComplete
                ? $"Database: migration '{e.Type.Name}' - Done"
                : $"Database: migration '{e.Type.Name}' - Failed");
        }
    }
}