namespace Game.Database.Core
{
    public abstract class DatabaseQueryBuilder
    {
        public static DatabaseQuery Table(string table)
        {
            return new DatabaseQuery(DatabaseQuery.Type.Select, table);
        }

        public static DatabaseQuery Insert(string table)
        {
            return new DatabaseQuery(DatabaseQuery.Type.Insert, table);
        }

        public static DatabaseQuery Exists(string table)
        {
            return Table(table).Exists();
        }

        public static DatabaseQuery NotExists(string table)
        {
            return Table(table).NotExists();
        }
    }
}
