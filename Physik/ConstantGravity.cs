using System.Numerics;

namespace Physik;

public class ConstantGravity : IPhysicsForce
{
    public Vector3 GravityVector { get; set; }

    public ConstantGravity(Vector3 gravity) => GravityVector = gravity;

    public void Apply(PhysicsBox body, float deltaTime)
    {
        if (!body.IsStatic)
        {
            body.Velocity += GravityVector * deltaTime;
        }
    }
}