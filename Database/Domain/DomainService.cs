using Game.Core;
using Game.Database.Domain.Repositories;

namespace Game.Database.Domain
{
    public partial class DomainService : IService
    {
        private static DomainService? _instance;
        public static DomainService Instance => _instance ??= new DomainService();

        private DomainService()
        {
            _repositories = new Dictionary<Type, Repository>();
        }

        public void Init()
        {
            InitRepositories();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }
        
        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}