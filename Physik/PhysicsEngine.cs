using System.Numerics;

namespace Physik;

public class PhysicsEngine
{
    private List<PhysicsBox> _bodies = new();

    public void Update(float deltaTime)
    {
        foreach (var body in _bodies)
        {
            if (body.IsStatic) continue;

            // 1. Schwerkraft oder andere Kräfte auf Velocity anwenden
            // 2. SweptAABB gegen alle anderen Boxen prüfen
            foreach (var obstacle in _bodies)
            {
                if (body == obstacle) continue;

                float hitTime = SweptAABB(body, obstacle, out Vector3 normal, deltaTime);

                if (hitTime < 1.0f) // Kollision erkannt!
                {
                    ResolveCollision(body, normal, hitTime);
                }
            }
            
            // 3. Position endgültig aktualisieren
            body.Position += body.Velocity * deltaTime;
        }
    }
}