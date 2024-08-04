using System.Data;

namespace Game.Database.Core
{
    public interface IDatabaseConnection : IDisposable
    {
        public IDataReader Read(string sql);
        public IDataReader Read(IDatabaseQuery databaseQuery);
        public void Execute(string sql);
        public void Execute(IDatabaseQuery databaseQuery);
        public bool Exists(string sql);
        public bool Exists(IDatabaseQuery databaseQuery);
    }
}
