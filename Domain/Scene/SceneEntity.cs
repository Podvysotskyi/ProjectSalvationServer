using System.Numerics;

namespace Game.Domain.Scene;

public partial class SceneEntity(string id)
{
    public readonly string Id = id;

    public Vector3 DefaultPosition { get; protected set; }
    public Quaternion DefaultRotation { get; protected set; }

    public void Load()
    {
        //TODO: read scene from file
        
        DefaultPosition = new Vector3();
        DefaultRotation = Quaternion.Identity;
    }

    public void Update()
    {
        UpdatePlayerPositions();
    }
}