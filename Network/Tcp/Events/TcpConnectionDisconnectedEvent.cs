namespace Game.Network.Tcp.Events;

public class TcpConnectionDisconnectedEvent(TcpConnection connection) : TcpConnectionEvent(connection);