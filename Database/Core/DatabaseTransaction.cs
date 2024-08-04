using System.Data;
using Game.Database.Exceptions;

namespace Game.Database.Core
{
    public class DatabaseTransaction : IDatabaseTransaction
    {
        private readonly DatabaseConnection _databaseConnection;

        public bool IsComplete { get; protected set; }

        public DatabaseTransaction(DatabaseConnection databaseConnection) {
            _databaseConnection = databaseConnection;
            databaseConnection.Execute("BEGIN TRANSACTION");
            IsComplete = false;
        }

        ~DatabaseTransaction()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!IsComplete)
            {
                Commit();
            }
        }

        public void Rollback()
        {
            Execute("ROLLBACK");
            IsComplete = true;
        }

        public void Commit()
        {
            Execute("COMMIT");
            IsComplete = true;
        }

        public void Execute(string sql)
        {
            if (IsComplete)
            {
                throw new DatabaseTransactionCompletedException(sql);
            }

            _databaseConnection.Execute(sql);
        }

        public void Execute(IDatabaseQuery databaseQuery)
        {
            Execute(databaseQuery.ToSql());
        }

        public bool Exists(string sql)
        {
            if (IsComplete)
            {
                throw new DatabaseTransactionCompletedException(sql);
            }

            return _databaseConnection.Exists(sql);
        }

        public bool Exists(IDatabaseQuery databaseQuery)
        {
            return Exists(databaseQuery.ToSql());
        }

        public IDataReader Read(string sql)
        {
            if (IsComplete)
            {
                throw new DatabaseTransactionCompletedException(sql);
            }

            return _databaseConnection.Read(sql);
        }

        public IDataReader Read(IDatabaseQuery databaseQuery)
        {
            return Read(databaseQuery.ToSql());
        }
    }
}
