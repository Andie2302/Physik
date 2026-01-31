using System.Numerics;

namespace Physik;

public abstract class PhysicsBox
{
    public Vector3 Position; 
    public Vector3 Size;
    public Vector3 Velocity;

    // Physikalische Basiseigenschaften
    public float Mass { get; init; } = 1.0f;
    public bool IsStatic { get; set; } 
    public float DragCoefficient { get; set; } = 1.05f; // Standardwert für Boxen

    // Die "Faulheit" (1/m)
    public float InverseMass => IsStatic || Mass <= 0 ? 0f : 1.0f / Mass;

    // Hilfsgrößen für die Kollision
    public Vector3 Min => Position;
    public Vector3 Max => Position + Size;
    
    // Flächen (Wichtig für Wind!)
    public float AreaX => Size.Y * Size.Z; // Seite
    public float AreaY => Size.X * Size.Z; // Oben/Unten
    public float AreaZ => Size.X * Size.Y; // Vorne/Hinten
}