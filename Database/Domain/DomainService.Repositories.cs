using Game.Database.Domain.Repositories;

namespace Game.Database.Domain
{
    public partial class DomainService
    {
        private readonly Dictionary<Type, Repository> _repositories;

        public void AddRepository<T>() where T : Repository
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type))
            {
                return;
            }

            var repository = Activator.CreateInstance(type) as Repository;
            if (repository == null)
            {
                return;
            }

            _repositories.Add(type, repository);
        }

        public T GetRepository<T>() where T : Repository
        {
            var type = typeof(T);
            var repository = _repositories[type] as T;

            if (repository == null)
            {
                throw new Exception($"Invalid repository type {type}");
            }

            return repository;
        }

        private void InitRepositories()
        {
            foreach (var type in _repositories.Keys)
            {
                _repositories[type].Init();
            }
        }
    }
}