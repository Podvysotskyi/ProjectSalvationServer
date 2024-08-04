namespace Game.Network.Tcp.Events
{
    public class ConnectionAcceptedEvent(Connection connection) : ConnectionEvent(connection);
}