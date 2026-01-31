using System.Numerics;

namespace Physik;

public class PhysicsBox
{
    // Position und Ausdehnung (AABB)
    public Vector3 Position; 
    public Vector3 Size;
    
    // Bewegung
    public Vector3 Velocity;
    
    // Hilfseigenschaften für die Berechnung
    public Vector3 Min => Position;
    public Vector3 Max => Position + Size;

    // Optionale Logik: Ist das Objekt fest (Boden) oder beweglich (Spieler)?
    public bool IsStatic { get; set; } 
}