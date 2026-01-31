using System.Numerics;

namespace Physik;

public class PhysicsWorld
{
    public Vector3 Gravity { get; set; } = new Vector3(0, -9.81f, 0);
    // Hier könnten später auch Wind oder Luftwiderstand stehen
}