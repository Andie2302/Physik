// See https://aka.ms/new-console-template for more information


using System.Numerics;
using Physik;

Console.WriteLine("Hello, World!");

// ... Engine Setup ...
var engine = new PhysicsEngine();

// 1. Schwerkraft (wirkt immer nach unten)
engine.GlobalForces.Add(new ConstantGravity(new Vector3(0, -9.81f, 0)));

// 2. Ein heftiger Sturm von links nach rechts (X-Richtung)
// Windgeschwindigkeit 20 m/s ist schon ein ordentlicher Sturm!
engine.GlobalForces.Add(new WindForce(new Vector3(20, 0, 0)));

// 3. Der Spieler (DynamicBox)
var player = new DynamicBox { 
    Position = new Vector3(0, 50, 0), // Startet weit oben
    Size = new Vector3(1, 2, 1),      // Eine Person ist höher als breit
    Mass = 80.0f                      // 80kg
};
engine.AddBody(player);

// 4. Ein Hindernis (StaticBox) - weiter rechts
engine.AddBody(new StaticBox { 
    Position = new Vector3(15, 0, 0), 
    Size = new Vector3(5, 10, 5) 
});

// Simulation für ein paar Sekunden laufen lassen und Positionen ausgeben...