// See https://aka.ms/new-console-template for more information


using System.Numerics;
using Physik;

Console.WriteLine("Hello, World!");


// 1. Setup: Welt und Engine initialisieren
var engine = new PhysicsEngine();

// 2. Kräfte hinzufügen (Schwerkraft)
var gravity = new ConstantGravity(new Vector3(0, -9.81f, 0));
engine.GlobalForces.Add(gravity);

// 3. Den Boden erstellen (Statisch)
var floor = new StaticBox 
{ 
    Position = new Vector3(0, 0, 0), 
    Size = new Vector3(10, 1, 10) 
};
engine.AddBody(floor);

// 4. Den Spieler erstellen (Beweglich)
var player = new DynamicBox 
{ 
    Position = new Vector3(0, 10, 0), // Startet in 10m Höhe
    Size = new Vector3(1, 1, 1) 
};
engine.AddBody(player);

Console.WriteLine("Simulation startet... Spieler fällt aus 10m Höhe.");
Console.WriteLine("Zeit | Position Y | Geschwindigkeit Y");
Console.WriteLine("-------------------------------------");

// 5. Simulations-Schleife (2 Sekunden in 1/60s Schritten)
float totalTime = 0;
float deltaTime = 1f / 60f;

for (int i = 0; i < 120; i++)
{
    engine.Update(deltaTime);
    totalTime += deltaTime;

    if (i % 5 == 0) // Nur jeden 5. Frame ausgeben
    {
        Console.WriteLine($"{totalTime:F2}s | {player.Position.Y:F2}m | {player.Velocity.Y:F2} m/s");
    }

    if (player.Position.Y <= 1.01f && player.Velocity.Y == 0)
    {
        Console.WriteLine("--- Aufprall erkannt und gelöst! ---");
        break;
    }
}

// Hilfsklassen, da PhysicsBox abstract ist
public class DynamicBox : PhysicsBox { public DynamicBox() => IsStatic = false; }
public class StaticBox : PhysicsBox { public StaticBox() => IsStatic = true; }