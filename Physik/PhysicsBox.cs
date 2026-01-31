using System.Numerics;

namespace Physik;

public abstract class PhysicsBox
{
    public Vector3 Position; 
    public Vector3 Size;

    public Vector3 Velocity;

    public Vector3 Min => Position;
    public Vector3 Max => Position + Size;

    public bool IsStatic { get; set; } 
}