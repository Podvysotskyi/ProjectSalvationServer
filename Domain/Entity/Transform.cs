using System.Numerics;

namespace Game.Domain.Entity;

public class Transform
{
    public Vector3 Position;

    public Quaternion Rotation;

    public Transform() : this(Vector3.Zero, Quaternion.Identity)
    {
    }
    
    public Transform(Vector3 position) : this(position, Quaternion.Identity)
    {
    }
    
    public Transform(Quaternion rotation) : this(Vector3.Zero, rotation)
    {
    }
    
    public Transform(Vector3 position, Quaternion rotation)
    {
        Position = position;
        Rotation = rotation;
    }
}