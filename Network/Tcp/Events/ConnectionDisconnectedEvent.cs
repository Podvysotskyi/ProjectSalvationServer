namespace Game.Network.Tcp.Events
{
    public class ConnectionDisconnectedEvent(Connection connection) : ConnectionEvent(connection);
}