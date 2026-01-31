using System.Numerics;

namespace Physik;

public class WindForce(Vector3 windVelocity, float airDensity = 1.225f) : IPhysicsForce
{
    public void Apply(PhysicsBox body, float deltaTime)
    {
        if (body.IsStatic) return;

        // 1. Relative Geschwindigkeit (Wie schnell weht der Wind am Objekt vorbei?)
        var relativeVelocity = windVelocity - body.Velocity;
        var speed = relativeVelocity.Length();
        if (speed <= 0.001f) return;

        var direction = Vector3.Normalize(relativeVelocity);

        // 2. Projizierte Fläche berechnen (Die Methode haben wir eben in PhysicsBox eingebaut)
        var area = body.GetProjectedArea(direction);

        // 3. Die physikalische Formel: F = 0.5 * rho * v² * cw * A
        var forceMagnitude = 0.5f * airDensity * (speed * speed) * body.DragCoefficient * area;
        var force = direction * forceMagnitude;

        // 4. Beschleunigung anwenden (a = F / m)
        body.Velocity += (force * body.InverseMass) * deltaTime;
    }
}

public class DynamicBox : PhysicsBox 
{ 
    public DynamicBox() 
    {
        IsStatic = false; // Erzwinge: Das Ding bewegt sich!
        Mass = 1.0f;      // Standardmasse setzen
    }
}

public class StaticBox : PhysicsBox 
{ 
    public StaticBox() 
    {
        IsStatic = true; // Erzwinge: Das Ding bewegt sich NIE!
        // Masse ist bei Static egal, da InverseMass dann eh 0 ist
    }
}