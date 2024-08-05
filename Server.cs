using Game.Core;
using Game.Database;
using Game.Engine;

namespace Game
{
    public class Server : IService
    {
        private bool _running;
        private readonly Mutex _mutex;
        private readonly Thread _thread;

        public Server()
        {
            _running = true;
            _mutex = new Mutex();
            _thread = new Thread(Run);
        }
        
        public void Init()
        {
            DatabaseService.Instance.Init();
            DomainManager.Init();
            SceneManager.Init();
            NetworkManager.Init();
        }

        public void Start()
        {
            DatabaseService.Instance.Start();
            DomainManager.Start();
            SceneManager.Start();
            NetworkManager.Start();
            
            _thread.Start();
        }

        public void Stop()
        {
            _mutex.WaitOne();
            _running = false;
            _mutex.ReleaseMutex();

            _thread.Join();
            
            NetworkManager.Stop();
        }

        private void Run()
        {
            var target = 1000 / 20;
            var time = DateTime.Now;
            
            while (true)
            {
                _mutex.WaitOne();
                if (!_running)
                {
                    _mutex.ReleaseMutex();
                    break;
                }
                _mutex.ReleaseMutex();

                {
                    NetworkManager.Tcp.Receive();
                    //TODO: Update
                    //TODO: Update physics
                    SceneManager.Update();
                    NetworkManager.Tcp.AcceptConnections();
                }

                var length = (int)(DateTime.Now - time).TotalMilliseconds;
                if (length < target)
                {
                    Thread.Sleep(target - length);
                }

                time = DateTime.Now;
            }
        }
    }
}