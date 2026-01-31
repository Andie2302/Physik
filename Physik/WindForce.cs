using System.Numerics;

namespace Physik;

public class WindForce(Vector3 windVelocity, float airDensity = 1.225f) : IPhysicsForce
{
    public void Apply(PhysicsBox body, float deltaTime)
    {
        if (body.IsStatic) return;

        // 1. Relative Geschwindigkeit (Wie schnell weht der Wind am Objekt vorbei?)
        Vector3 relativeVelocity = windVelocity - body.Velocity;
        float speed = relativeVelocity.Length();
        if (speed <= 0.001f) return;

        Vector3 direction = Vector3.Normalize(relativeVelocity);

        // 2. Projizierte Fläche berechnen (Die Methode haben wir eben in PhysicsBox eingebaut)
        float area = body.GetProjectedArea(direction);

        // 3. Die physikalische Formel: F = 0.5 * rho * v² * cw * A
        float forceMagnitude = 0.5f * airDensity * (speed * speed) * body.DragCoefficient * area;
        Vector3 force = direction * forceMagnitude;

        // 4. Beschleunigung anwenden (a = F / m)
        body.Velocity += (force * body.InverseMass) * deltaTime;
    }
}