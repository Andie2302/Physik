namespace Physik;

public interface IPhysicsForce
{
    void Apply(PhysicsBox body, float deltaTime);
}