using System.Net.Sockets;

namespace Game.Network.Tcp.Threads
{
    public class AcceptConnectionThread(System.Net.Sockets.Socket socket) : SocketWorker(socket)
    {
        private readonly List<System.Net.Sockets.Socket> _sockets = new();

        public System.Net.Sockets.Socket? Accept()
        {
            Lock();
            try
            {
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
                
                var socket = Socket.Accept();
                
                Lock();
                if (IsRunning)
                {
                    _sockets.Add(socket);
                }
                Unlock();
            }
        }
    }
}