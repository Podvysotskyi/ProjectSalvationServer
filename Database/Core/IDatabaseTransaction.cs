namespace Game.Database.Core
{
    public interface IDatabaseTransaction : IDatabaseConnection
    {
        public void Rollback();
        public void Commit();
    }
}
