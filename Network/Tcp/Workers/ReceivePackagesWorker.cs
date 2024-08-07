using Game.Helpers;
using Game.Network.Package;

namespace Game.Network.Tcp.Workers;

public class ReceivePackagesWorker(System.Net.Sockets.Socket socket) : SocketWorker(socket)
{
    private readonly List<byte[]> _buffer = new();

    protected override void OnStop()
    {
        _buffer.Clear();
    }

    public List<NetworkPackage> Receive()
    {
        Lock();
        if (_buffer.Count == 0)
        {
            Unlock();
            return [];
        }
            
        var packages = new List<NetworkPackage>();
        for (var i = 0; i < _buffer.Count; i++)
        {
            var buffer = _buffer[i];
            var position = 0;

            var type = (NetworkPackageType)BitConverterHelper.ReadUShort(buffer, ref position);
            var length = BitConverterHelper.ReadUShort(buffer, ref position);
            try
            {
                NetworkPackage? package;
                if (length > 0)
                {
                    var data = BitConverterHelper.ReadBytes(buffer, length, ref position);
                    package = NetworkPackageFacade.CreatePackage(type, data);
                }
                else
                {
                    package = NetworkPackageFacade.CreatePackage(type);
                }

                packages.Add(package);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Fail to read '{type}' package: {e}");
            }
        }

        _buffer.Clear();
            
        Unlock();

        return packages;
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
            
            var data = new byte[1024];
            try
            {
                var length = Socket.Receive(data);
                
                if (length >= 4)
                {
                    Lock();
                    if (IsRunning)
                    {
                        _buffer.Add(data);
                    }
                    Unlock();
                }
            }
            catch
            {
                Stop();
                break;
            }
        }
    }
}