using System.Numerics;

namespace Physik;

public class ConstantGravity(Vector3 gravity) : IPhysicsForce
{
    private Vector3 GravityVector { get; set; } = gravity;

    public void Apply(PhysicsBox body, float deltaTime)
    {
        if (!body.IsStatic)
        {
            body.Velocity += GravityVector * deltaTime;
        }
    }
}