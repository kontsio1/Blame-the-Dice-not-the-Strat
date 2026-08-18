// Simulation.cs

using axis_console_project.Armies;
using axis_console_project.Battles;

namespace axis_console_project.Simulations;

public class Simulation
{
    public SimulationStats Stats { get; private set; }
    private Army AttackingArmy { get; }
    private Army DefendingArmy { get; }
    public Simulation(Army army1, Army army2)
    {
        if (army1.IsAttacking && !army2.IsAttacking || !army1.IsAttacking && army2.IsAttacking)
        {
            AttackingArmy = army1.IsAttacking ? army1 : army2;
            DefendingArmy = !army2.IsAttacking ? army2 : army1;
            
            Stats = new SimulationStats(AttackingArmy, DefendingArmy);
        }
        else throw new Exception("One army must be attacking and the other defending");
    }
    public int Progress = 0;

    public override string ToString()
    {
        return
            $"\nSimulation for armies:\n{AttackingArmy.Units} \nvs\n {DefendingArmy.Units}\n";
    }

    public List<BattleResult> Run(int numberOfSimulations = 1000)
    {
        var battleResults = new List<BattleResult>();
        for (int i = 0; i < numberOfSimulations; i++)
        {
            var battleResult = RunOnce();
            battleResults.Add(battleResult);
            Progress = i / numberOfSimulations;
        }
        return battleResults;
    }

    public BattleResult RunOnce()
    {
        // Console.Write($"\r--- {(double)i / NumberOfSimulations * 100:F2}% Complete ---");
        var battle = new Battle(AttackingArmy.Clone(), DefendingArmy.Clone());
        var battleResult = battle.Fight();
        Stats.RecordResult(battleResult);
        
        Progress = 1;
        return battleResult;
    }

    public void Reset()
    {
        Stats = new SimulationStats(AttackingArmy, DefendingArmy);
        Progress = 0;
    }
}