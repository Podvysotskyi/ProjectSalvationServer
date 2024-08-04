namespace Game.Database.Events
{
    public abstract class DatabaseMigrationEvent(Type type)
    {
        public readonly Type Type = type;
    }
}
