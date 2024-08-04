namespace Game.Network.Tcp.Events
{
    public abstract class ConnectionEvent(Connection connection)
    {
        public string Id => Connection.Id;
        public readonly Connection Connection = connection;
    }
}