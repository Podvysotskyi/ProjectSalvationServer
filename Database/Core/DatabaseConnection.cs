using System.Data;
using Game.Database.Events;
using Game.Database.Exceptions;
using Microsoft.Data.Sqlite;

namespace Game.Database.Core
{
    public class DatabaseConnection : IDatabaseConnection
    {
        private readonly IDbConnection _connection;

        public bool IsOpen { get; set; }

        public event EventHandler<DatabaseQueryEvent>? DatabaseQueryEvent;

        public DatabaseConnection(string filename)
        {
            _connection = new SqliteConnection($"Data Source={filename}");
            _connection.Open();
            IsOpen = true;
        }

        ~DatabaseConnection()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!IsOpen)
            {
                return;
            }

            _connection.Close();
            IsOpen = false;
        }

        public DatabaseTransaction StartTransaction()
        {
            if (!IsOpen)
            {
                throw new DatabaseConnectionClosedException();
            }
            
            return new DatabaseTransaction(this);
        }

        public IDataReader Read(string sql)
        {
            if (!IsOpen)
            {
                throw new DatabaseConnectionClosedException();
            }
            
            var command = _connection.CreateCommand();
            command.CommandText = sql;

            DatabaseQueryEvent?.Invoke(this, new DatabaseQueryEvent(sql));

            return command.ExecuteReader();
        }

        public IDataReader Read(IDatabaseQuery databaseQuery)
        {
            return Read(databaseQuery.ToSql());
        }

        public void Execute(string sql)
        {
            if (!IsOpen)
            {
                throw new DatabaseConnectionClosedException();
            }
            
            IDbCommand command;
            using (command = _connection.CreateCommand())
            {
                command.CommandText = sql;

                DatabaseQueryEvent?.Invoke(this, new DatabaseQueryEvent(sql));

                command.ExecuteNonQuery();
            }
        }

        public void Execute(IDatabaseQuery databaseQuery)
        {
            Execute(databaseQuery.ToSql());
        }

        public bool Exists(string sql)
        {
            if (!IsOpen)
            {
                throw new DatabaseConnectionClosedException();
            }
            
            var result = false;

            IDbCommand command;
            using (command = _connection.CreateCommand())
            {
                command.CommandText = sql;

                DatabaseQueryEvent?.Invoke(this, new DatabaseQueryEvent(sql));

                IDataReader reader;
                using (reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public bool Exists(IDatabaseQuery databaseQuery)
        {
            return Exists(databaseQuery.ToSql());
        }
    }
}
