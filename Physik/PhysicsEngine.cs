using System.Numerics;

namespace Physik;

public class PhysicsEngine
{
    private readonly List<PhysicsBox> _bodies = [];

    public List<IPhysicsForce> GlobalForces { get; } = [];

    public void AddBody(PhysicsBox body) => _bodies.Add(body);
    public void RemoveBody(PhysicsBox body) => _bodies.Remove(body);
    
    public void Update(float deltaTime)
    {
        foreach (var body in _bodies)
        {
            if (body.IsStatic) continue;

            foreach (var force in GlobalForces)
            {
                force.Apply(body, deltaTime);
            }

            foreach (var obstacle in _bodies)
            {
                if (body == obstacle) continue;

                var hitTime = SweptAabb(body, obstacle, out var normal, deltaTime);

                if (hitTime < 1.0f) 
                {
                    ResolveCollision(body, normal, hitTime, deltaTime);
                }
            }
            
            body.Position += body.Velocity * deltaTime;
        }
    }

    private static float SweptAabb(PhysicsBox b1, PhysicsBox b2, out Vector3 normal, float deltaTime)
    {
        normal = Vector3.Zero;
        var velocityFrame = b1.Velocity * deltaTime;

        Vector3 entry, exit;

        CalculateAxis(velocityFrame.X, b1.Min.X, b1.Max.X, b2.Min.X, b2.Max.X, out entry.X, out exit.X);
        CalculateAxis(velocityFrame.Y, b1.Min.Y, b1.Max.Y, b2.Min.Y, b2.Max.Y, out entry.Y, out exit.Y);
        CalculateAxis(velocityFrame.Z, b1.Min.Z, b1.Max.Z, b2.Min.Z, b2.Max.Z, out entry.Z, out exit.Z);

        var entryTime = Math.Max(entry.X, Math.Max(entry.Y, entry.Z));
        var exitTime = Math.Min(exit.X, Math.Min(exit.Y, exit.Z));

        if (entryTime > exitTime || (entry.X < 0.0f && entry.Y < 0.0f && entry.Z < 0.0f) || entryTime > 1.0f)
            return 1.0f;

        if (entry.X > entry.Y && entry.X > entry.Z)
            normal = new Vector3(velocityFrame.X < 0 ? 1 : -1, 0, 0);
        else if (entry.Y > entry.X && entry.Y > entry.Z)
            normal = new Vector3(0, velocityFrame.Y < 0 ? 1 : -1, 0);
        else
            normal = new Vector3(0, 0, velocityFrame.Z < 0 ? 1 : -1);

        return entryTime;
    }

    private static void CalculateAxis(float vel, float min1, float max1, float min2, float max2, out float entry, out float exit)
    {
        switch (vel)
        {
            case > 0:
                entry = (min2 - max1) / vel;
                exit = (max2 - min1) / vel;
                break;
            case < 0:
                entry = (max2 - min1) / vel;
                exit = (min2 - max1) / vel;
                break;
            default:
                entry = float.NegativeInfinity;
                exit = float.PositiveInfinity;
                break;
        }
    }

    private static void ResolveCollision(PhysicsBox body, Vector3 normal, float hitTime, float deltaTime)
    {
        body.Position += body.Velocity * deltaTime * (hitTime - 0.001f);

        if (Math.Abs(normal.X) > 0) body.Velocity.X = 0;
        if (Math.Abs(normal.Y) > 0) body.Velocity.Y = 0;
        if (Math.Abs(normal.Z) > 0) body.Velocity.Z = 0;
    }
    
    public void Apply(PhysicsBox body, float deltaTime)
    {
        // 1. Relative Geschwindigkeit (Windgeschwindigkeit - Objektgeschwindigkeit)
        Vector3 relativeWind = _globalWindVelocity - body.Velocity;
        float speedSq = relativeWind.LengthSquared();
        if (speedSq <= 0) return;

        // 2. Vereinfachung: Wir nehmen die Fläche, die der Hauptwindrichtung am nächsten ist
        float effectiveArea = CalculateProjectedArea(body, Vector3.Normalize(relativeWind));

        // 3. F = 0.5 * Luftdichte * v² * Area * cw
        float forceMag = 0.5f * 1.225f * speedSq * effectiveArea * body.DragCoefficient;
        Vector3 force = Vector3.Normalize(relativeWind) * forceMag;

        body.Velocity += (force * body.InverseMass) * deltaTime;
    }
}