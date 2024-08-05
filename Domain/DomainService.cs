using Game.Core;
using Game.Domain.Player;
using Game.Domain.User;

namespace Game.Domain;

public class DomainService : IService
{
    private readonly Dictionary<Type, Repository> _repositories;

    public readonly PlayerService PlayerService = new();
    
    public DomainService()
    {
        _repositories = new Dictionary<Type, Repository>();
        
        AddRepository<UserRepository>();
    }

    public void Init()
    {
        foreach (var type in _repositories.Keys)
        {
            _repositories[type].Init();
        }
        
        PlayerService.Init();
    }

    public void Start()
    {
        PlayerService.Start();
    }
        
    public void Stop()
    {
        PlayerService.Stop();
    }
    
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
}