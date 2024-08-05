namespace Game.Network.Tcp.Events;

public class TcpConnectionEvent(TcpConnection connection)
{
    public string Id => Connection.Id;
    public readonly TcpConnection Connection = connection;
}