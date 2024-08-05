using Game.Core;
using Game.Domain;
using Game.Domain.Player;

namespace Game.Engine;

public class DomainManager : ServiceManager<DomainService>
{
    public static PlayerService PlayerService => Instance.PlayerService;
    
    public static T Repository<T>() where T : Repository
    {
        return Instance.GetRepository<T>();
    }
}