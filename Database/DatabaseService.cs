using Game.Core;
using Game.Database.Core;
using Game.Database.Events;

namespace Game.Database
{
    public partial class DatabaseService : IService
    {
        private static DatabaseService? _instance;
        public static DatabaseService Instance => _instance ??= new DatabaseService();

        public bool IsReady { get; private set; }
        public event EventHandler<DatabaseReadyEvent>? DatabaseReadyEvent;
        
        private readonly string _filename;

        private DatabaseService()
        {
            _filename = "Database.db";
            _migrations = new List<Type>();
            IsReady = false;
        }

        public void Init()
        {
            DatabaseReadyEvent += OnDatabaseReady;
            DatabaseStartMigrationEvent += OnDatabaseMigrationStart;
            DatabaseMigrationCompleteEvent += OnDatabaseMigrationComplete;
            
            Console.WriteLine("Database: init");
        }

        public void Start()
        {
            Console.WriteLine("Database: start");
            
            RunMigrations();
            
            IsReady = true;
            DatabaseReadyEvent?.Invoke(this, new DatabaseReadyEvent());
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public DatabaseConnection OpenConnection()
        {
            var connection = new DatabaseConnection(_filename);

            connection.DatabaseQueryEvent += OnDatabaseQuery;

            return connection;
        }
        
        private static void OnDatabaseQuery(object? sender, DatabaseQueryEvent e)
        {
            Console.WriteLine($"Database: {e.Query}");
        }

        private static void OnDatabaseReady(object? sender, DatabaseReadyEvent e)
        {
            Console.WriteLine($"Database: ready");
        }
    }
}