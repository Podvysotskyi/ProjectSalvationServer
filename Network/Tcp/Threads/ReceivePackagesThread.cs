using System.Net.Sockets;
using Game.Helpers;
using Game.Network.Domain;
using Game.Network.Domain.Enums;
using Game.Network.Domain.Packages;

namespace Game.Network.Tcp.Threads
{
    public class ReceivePackagesThread(System.Net.Sockets.Socket socket) : SocketWorker(socket)
    {
        private readonly List<byte[]> _buffer = new();

        protected override void OnStop()
        {
            _buffer.Clear();
        }

        public Package[] Receive()
        {
            Lock();
            if (_buffer.Count == 0)
            {
                Unlock();
                return [];
            }
            
            var packages = new List<Package>();
            foreach (var buffer in _buffer)
            {
                var position = 0;

                try
                {
                    var type = (PackageType)BitConverterHelper.ReadUShort(buffer, ref position);
                    var length = BitConverterHelper.ReadUShort(buffer, ref position);

                    Package? package;
                    if (length > 0)
                    {
                        var data = BitConverterHelper.ReadBytes(buffer, length, ref position);
                        package = PackageService.Instance.CreatePackage(type, data);
                    }
                    else
                    {
                        package = PackageService.Instance.CreatePackage(type);
                    }

                    if (package == null)
                    {
                        throw new Exception();
                    }
                    
                    packages.Add(package);
                }
                catch
                {
                    Unlock();
                    throw new Exception("Fail to read a package");
                }
            }
            _buffer.Clear();
            
            Unlock();

            return packages.ToArray();
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
        }
    }
}