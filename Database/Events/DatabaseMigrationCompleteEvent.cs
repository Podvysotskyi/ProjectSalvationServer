namespace Game.Database.Events
{
    public class DatabaseMigrationCompleteEvent(Type type, bool isComplete) : DatabaseMigrationEvent(type)
    {
        public readonly bool IsComplete = isComplete;
    }
}