using System.Numerics;
using Physik;

// 1. Setup: Welt und Engine initialisieren
var engine = new PhysicsEngine();

// 2. Kräfte hinzufügen
// Schwerkraft
engine.GlobalForces.Add(new ConstantGravity(new Vector3(0, -9.81f, 0)));
// Wind (optional, zum Testen)
engine.GlobalForces.Add(new WindForce(new Vector3(10, 0, 0))); 

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
    Size = new Vector3(1, 2, 1),      // Höhe 2m, Breite 1m (gut für Wind-Test)
    Mass = 80f
};
engine.AddBody(player);

Console.WriteLine("Simulation startet... Spieler fällt aus 10m Höhe.");
Console.WriteLine("Zeit | Position Y | Geschwindigkeit Y");
Console.WriteLine("-------------------------------------");

// 5. Simulations-Schleife (2 Sekunden in 1/60s Schritten)
float totalTime = 0;
const float deltaTime = 1f / 60f;

for (var i = 0; i < 120; i++)
{
    engine.Update(deltaTime);
    totalTime += deltaTime;

    // Nur jeden 5. Frame ausgeben, damit die Konsole lesbar bleibt
    if (i % 5 == 0) 
    {
        Console.WriteLine($"{totalTime:F2}s | Y: {player.Position.Y:F2}m | vY: {player.Velocity.Y:F2}");
    }

    // Abbruchbedingung: Wenn der Spieler am Boden liegt
    if (player.Position.Y <= 1.01f && Math.Abs(player.Velocity.Y) < 0.1f)
    {
        Console.WriteLine("--- Aufprall erkannt und gelöst! ---");
        break;
    }
}