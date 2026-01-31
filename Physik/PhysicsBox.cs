using System.Numerics;

namespace Physik;

public abstract class PhysicsBox
{
    public Vector3 Position; 
    public Vector3 Size;
    public Vector3 Velocity;

    public float Mass { get; init; } = 1.0f;
    public bool IsStatic { get; set; } 
    public float DragCoefficient { get; set; } = 1.05f; // Standard für Boxen

    public float InverseMass => IsStatic || Mass <= 0 ? 0f : 1.0f / Mass;

    public Vector3 Min => Position;
    public Vector3 Max => Position + Size;
    
    // Hilfsmethode: Gibt die Fläche zurück, die in eine bestimmte Richtung zeigt
    public float GetProjectedArea(Vector3 direction)
    {
        // Wir nehmen den absoluten Wert der Richtung, um die Fläche zu bestimmen
        var dir = Vector3.Abs(direction);
        // Wir gewichten die Flächen der Achsen basierend auf der Windrichtung
        return (dir.X * (Size.Y * Size.Z)) + 
               (dir.Y * (Size.X * Size.Z)) + 
               (dir.Z * (Size.X * Size.Y));
    }
}