using System.Net.Sockets;

namespace Game.Network.Tcp.Threads
{
    public abstract class SocketWorker : IDisposable
    {
        protected bool IsRunning { get; private set; }

        protected System.Net.Sockets.Socket Socket { get; private set; }
        private readonly Thread _thread;
        private readonly Mutex _mutex;

        protected SocketWorker(System.Net.Sockets.Socket socket)
        {
            IsRunning = false;
            Socket = socket;

            _mutex = new Mutex();
            _thread = new Thread(Handle);
        }

        public void Lock()
        {
            _mutex.WaitOne();
        }
        
        public void Unlock()
        {
            _mutex.ReleaseMutex();
        }

        ~SocketWorker()
        {
            Dispose();
        }

        public void Dispose()
        {
            Stop();
        }

        public void Start()
        {
            Lock();
            
            if (!IsRunning)
            {
                IsRunning = true;
                OnStart();
                _thread.Start();
            }

            Unlock();
        }

        protected virtual void OnStart()
        {
        }

        public void Stop()
        {
            Lock();

            if (IsRunning)
            {
                IsRunning = false;
                OnStop();
            }

            Unlock();
        }

        protected virtual void OnStop()
        {
        }

        protected abstract void Handle();
    }
}