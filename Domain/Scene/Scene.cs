using System.Numerics;

namespace Game.Domain.Scene;

public partial class Scene
{
    public readonly string Id;

    public Scene(string id)
    {
        Id = id;
    }

    public Vector3 DefaultPosition { get; private set; }
    public Quaternion DefaultRotation { get; private set; }

    public void Load()
    {
        //TODO: read scene from file
        
        DefaultPosition = new Vector3();
        DefaultRotation = Quaternion.Identity;
    }
}