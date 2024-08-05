using System.Numerics;

namespace Game.Domain.Entity;

public class Transform(Vector3 position, Quaternion rotation)
{
    public Vector3 Position { get; private set; } = position;
    public Quaternion Rotation { get; private set; } = rotation;

    public Transform() : this(new Vector3(0, 0, 0), Quaternion.Identity)
    {
    }
    
    public Transform(Vector3 position) : this(position, Quaternion.Identity)
    {
    }
    
    public Transform(Quaternion rotation) : this(new Vector3(0, 0, 0), rotation)
    {
    }

    public void SetPosition(Vector3 position)
    {
        Position = position;
    }

    public void SetRotation(Quaternion rotation)
    {
        Rotation = rotation;
    }
}