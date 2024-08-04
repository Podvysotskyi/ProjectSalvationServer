namespace Game.Database.Events
{
    public class DatabaseQueryEvent(string query)
    {
        public readonly string Query = query;
    }
}
