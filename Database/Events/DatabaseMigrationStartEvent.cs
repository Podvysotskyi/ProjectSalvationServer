namespace Game.Database.Events
{
    public class DatabaseMigrationStartEvent(Type type) : DatabaseMigrationEvent(type);
}
