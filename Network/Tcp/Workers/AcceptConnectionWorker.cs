using System.Net.Sockets;

namespace Game.Network.Tcp.Workers;

public class AcceptConnectionWorker(Socket socket) : SocketWorker(socket)
{
    private readonly List<Socket> _sockets = new();

    public Socket? Accept()
    {
        Lock();
        try
        {
            if (_sockets.Count == 0)
            {
                return null;
            }
            
            var socket = _sockets.First();
            _sockets.Remove(socket);
            return socket;
        }
        catch
        {
            return null;
        }
        finally
        {
            Unlock();
        }
    }

    protected override void OnStop()
    {
        foreach (var socket in _sockets)
        {
            socket.Shutdown(SocketShutdown.Both);
            socket.Close();
        }

        _sockets.Clear();
    }

    protected override void Handle()
    {
        while (true)
        {
            Lock();
            if (!IsRunning)
            {
                Unlock();
                break;
            }
            Unlock();

            try
            {
                var socket = Socket.Accept();
                    
                Lock();
                if (IsRunning)
                {
                    _sockets.Add(socket);
                }
                Unlock();
            }
            catch
            {
                break;
            }
        }
    }
}