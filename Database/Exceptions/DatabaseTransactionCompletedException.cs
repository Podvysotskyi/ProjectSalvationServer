namespace Game.Database.Exceptions
{
    public class DatabaseTransactionCompletedException(string sql)
        : Exception("Database transaction is already completed")
    {
        public readonly string Sql = sql;
    }
}
