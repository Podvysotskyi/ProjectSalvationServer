using Game.Helpers;
using Game.Network.Package;

namespace Game.Network.Tcp.Workers;

public class SendPackagesWorker(System.Net.Sockets.Socket socket) : SocketWorker(socket)
{
    private readonly List<byte[]> _buffer = new();

    public void Send(NetworkPackage networkPackage)
    {
        var buffer = new List<byte>();
        buffer.AddRange(BitConverterHelper.ToArray(networkPackage.Type));
        var data = networkPackage.ToArray();
        buffer.AddRange(BitConverterHelper.ToArray((ushort)data.Length));

        if (data.Length > 0)
        {
            buffer.AddRange(data);
        }

        Lock();
        _buffer.Add(buffer.ToArray());
        Unlock();
    }

    protected override void OnStop()
    {
        _buffer.Clear();
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

            if (_buffer.Count > 0)
            {
                var data = _buffer.First();
                _buffer.Remove(data);
                    
                Unlock();

                try
                {
                    Socket.Send(data);
                }
                catch
                {
                    Lock();
                    Stop();
                    Unlock();
                    break;
                }
            }
            else
            {
                Unlock();
            }
        }
    }
}