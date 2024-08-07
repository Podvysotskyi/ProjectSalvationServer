namespace Game.Database.Core
{
    public class DatabaseQuery : IDatabaseQuery
    {
        public enum Type : int
        {
            Select,
            Insert,
        }

        private enum ExistsType : int
        {
            Ignore,
            Exists,
            NotExists,
        }

        private int _limit = 0;
        private readonly Type _type;
        private string _table;
        private bool _distinct = false;
        private ExistsType _exists = ExistsType.Ignore;
        private readonly Dictionary<string, string> _data = new();
        private readonly List<string> _conditions = new();
        private readonly List<string> _columns = new();

        public DatabaseQuery(Type type, string table)
        {
            _type = type;
            _table = table;
            _columns.Add("*");
        }
        
        public DatabaseQuery AddSelect(string column)
        {
            _columns.Add(column);
            return this;
        }

        public DatabaseQuery Select(string column)
        {
            _columns.Clear();
            return AddSelect(column);
        }
        
        public DatabaseQuery AddSelect(string[] columns)
        {
            _columns.AddRange(columns);
            return this;
        }

        public DatabaseQuery Select(string[] columns)
        {
            _columns.Clear();
            return AddSelect(columns);
        }

        public DatabaseQuery Distinct()
        {
            _distinct = true;
            return this;
        }

        public DatabaseQuery Limit(int value)
        {
            _limit = value;
            return this;
        }

        public DatabaseQuery Where(string column, string action, string value)
        {
            _conditions.Add($"{column} {action} '{value}'");
            return this;
        }

        public DatabaseQuery Where(string column, string action, int value)
        {
            _conditions.Add($"{column} {action} {value}");
            return this;
        }

        public DatabaseQuery Where(string column, string value)
        {
            return Where(column, "=", value);
        }

        public DatabaseQuery WhereNot(string column, string value)
        {
            return Where(column, "!=", value);
        }

        public DatabaseQuery WhereNull(string column)
        {
            _conditions.Add($"{column} IS NULL");
            return this;
        }

        public DatabaseQuery WhereNotNull(string column)
        {
            _conditions.Add($"{column} IS NOT NULL");
            return this;
        }

        public DatabaseQuery WhereIn(string column, string[] values)
        {
            var str = string.Join("', '", values);
            _conditions.Add($"{column} IN ('${str})'");
            return this;
        }

        public DatabaseQuery WhereIn(string column, int[] values)
        {
            var str = string.Join("', '", values);
            _conditions.Add($"{column} IN ('${str}')");
            return this;
        }

        public DatabaseQuery Exists()
        {
            _exists = ExistsType.Exists;
            return this;
        }

        public DatabaseQuery NotExists()
        {
            _exists = ExistsType.NotExists;
            return this;
        }

        public DatabaseQuery Data(string column, string value)
        {
            _data.Add(column, $"'{value}'");
            return this;
        }

        public DatabaseQuery Data(string column, int value)
        {
            _data.Add(column, value.ToString());
            return this;
        }

        public DatabaseQuery Table(string table)
        {
            _table = table;
            return this;
        }

        public string ToSql()
        {
            var sql = "";

            if (_type == Type.Insert)
            {
                sql =
                    $"INSERT INTO {_table} ({string.Join(", ", _data.Keys)}) VALUES({string.Join(", ", _data.Values)})";
            }
            else if (_type == Type.Select)
            {
                sql = $"SELECT {(_distinct ? "DISTINCT " : "")} {string.Join(", ", _columns)} FROM {_table}";

                for (int i = 0; i < _conditions.Count; i++)
                {
                    sql += $" {(i == 0 ? "WHERE" : "AND")} {_conditions[i]}";
                }

                if (_limit > 0)
                {
                    sql += $" LIMIT {_limit}";
                }

                if (_exists == ExistsType.NotExists)
                {
                    sql = $"NOT EXISTS ({sql})";
                }
                else if (_exists == ExistsType.Exists)
                {
                    sql = $"EXISTS ({sql})";
                }
            }

            return sql;
        }
    }
}
