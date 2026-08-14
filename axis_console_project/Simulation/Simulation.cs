// Simulation.cs

namespace axis_console_project.Simulation;

public class Simulation(Army.Army attackingArmy, Army.Army defendingArmy, int numberOfSimulations)
{
    private SimulationStats Stats { get; set; } = new();
    private Army.Army AttackingArmy { get; } = attackingArmy;
    private Army.Army DefendingArmy { get; } = defendingArmy;
    private int NumberOfSimulations { get; set; } = numberOfSimulations;

    public override string ToString()
    {
        return $"\nSimulation for armies:\n{AttackingArmy.Units} \nvs\n {DefendingArmy.Units} \nrunning {NumberOfSimulations} simulations.\n";
    }

    public SimulationStats Run()
    {
        Stats = new SimulationStats
        {
            AttackingArmy = AttackingArmy,
            DefendingArmy = DefendingArmy,
        };
        for (int i = 0; i < NumberOfSimulations; i++)
        {
            // Console.Write($"\r--- {(double)i / NumberOfSimulations * 100:F2}% Complete ---");
            var battle = new Battle.Battle(AttackingArmy.Clone(), DefendingArmy.Clone());
            var battleResult = battle.Fight();
            Stats.RecordResult(battleResult);
        }

        return Stats;
    }
}