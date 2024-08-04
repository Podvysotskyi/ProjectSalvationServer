namespace Game.Database.Core
{
    public class DatabaseTableBuilder : IDatabaseQuery
    {
        private readonly string _table;
        private readonly Dictionary<string, string> _columns = new();

        private DatabaseTableBuilder(string table)
        {
            _table = table;
        }

        public DatabaseTableBuilder AddColumn(string name, string type)
        {
            _columns.Add(name, $"{name} {type}");
            return this;
        }

        public string ToSql()
        {
            return $"CREATE TABLE IF NOT EXISTS {_table} ({string.Join(", ", _columns.Values)})";
        }

        public static DatabaseTableBuilder Table(string table)
        {
            return new DatabaseTableBuilder(table);
        }
    }
}
