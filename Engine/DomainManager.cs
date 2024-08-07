using Game.Core;
using Game.Domain;

namespace Game.Engine;

public class DomainManager : ServiceManager<DomainService>
{
    public static T Repository<T>() where T : Repository
    {
        return Instance.GetRepository<T>();
    }
}