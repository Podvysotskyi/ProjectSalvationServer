using Game.Core;
using Game.Database;
using DatabaseDomain = Game.Database.Domain;
using TCP = Game.Network.Tcp;

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
    
            DatabaseDomain.DomainService.Instance.AddRepository<DatabaseDomain.Repositories.UserRepository>();
            DatabaseDomain.DomainService.Instance.Init();
    
            TCP.NetworkService.Instance.Init();
        }

        public void Start()
        {
            DatabaseService.Instance.Start();
            TCP.NetworkService.Instance.Start();
            
            
        }

        public void Stop()
        {
            _mutex.WaitOne();
            _running = false;
            _mutex.ReleaseMutex();

            _thread.Join();
            
            TCP.NetworkService.Instance.Stop();
        }

        private void Run()
        {
            const int fps = 1000 / 20;

            while (true)
            {
                var ticks = DateTime.Now.Ticks;
                _mutex.WaitOne();
                if (!_running)
                {
                    _mutex.ReleaseMutex();
                    break;
                }
                _mutex.ReleaseMutex();
                Console.Write('.');

                {
                    //TODO: Update physics
                    //TODO: Receive network packages
                    //TODO: Update
                    //TODO: Send network packages
                    TCP.NetworkService.Instance.AcceptConnections();
                }

                var diff = (int)((DateTime.Now.Ticks - ticks) / 1000);
                if (diff < fps)
                {
                    Thread.Sleep(diff);
                }
            }
        }
    }
}