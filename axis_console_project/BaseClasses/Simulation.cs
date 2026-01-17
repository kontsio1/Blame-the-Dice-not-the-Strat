using static Army;

public class Simulation(Army attackingArmy, Army defendingArmy, int numberOfSimulations)
{
    public SimulationStats Stats { get; set; }
    public Army AttackingArmy { get; set; } = attackingArmy;
    public Army DefendingArmy { get; set; } = defendingArmy;
    public int NumberOfSimulations { get; set; } = numberOfSimulations;

    public override string ToString()
    {
        return $"\nSimulation for armies:\n{AttackingArmy.units} \nvs\n {DefendingArmy.units} \nrunning {NumberOfSimulations} simulations.\n";
    }

    public SimulationStats Run()
    {
        Stats = new SimulationStats();

        for (int i = 0; i < NumberOfSimulations; i++)
        {
            Console.WriteLine($"\n--- {(double)i / NumberOfSimulations * 100:F2}% Complete ---\n");
            var battle = new Battle(attackingArmy.Clone(), defendingArmy.Clone());
            var battleResult = battle.Fight();
            Stats.RecordResult(battleResult);
        }

        return Stats;
    }
}

public class SimulationStats
{
    public int AttackerWon { get; set; }
    public int DefenderWon { get; set; }
    public int Draw { get; set; }
    public int TotalBattles => AttackerWon + DefenderWon + Draw;
    public double AttackerWonPercentage =>
        TotalBattles == 0 ? 0 : (AttackerWon * 100.0) / TotalBattles;
    public double DefenderWonPercentage =>
        TotalBattles == 0 ? 0 : (DefenderWon * 100.0) / TotalBattles;
    public double DrawPercentage => TotalBattles == 0 ? 0 : (Draw * 100.0) / TotalBattles;
    private List<Units> AttackerRemainingUnits { get; set; } = new List<Units>();
    private List<Units> DefenderRemainingUnits { get; set; } = new List<Units>();
    public UnitsStats AttackerRemainingUnitsAvg =>
        new UnitsStats(
            AttackerRemainingUnits.Count == 0
                ? 0
                : AttackerRemainingUnits.Average(u => u.InfantryUnits.Count),
            AttackerRemainingUnits.Count == 0
                ? 0
                : AttackerRemainingUnits.Average(u => u.ArtilleryUnits.Count),
            AttackerRemainingUnits.Count == 0
                ? 0
                : AttackerRemainingUnits.Average(u => u.TankUnits.Count),
            AttackerRemainingUnits.Count == 0
                ? 0
                : AttackerRemainingUnits.Average(u => u.FighterUnits.Count),
            AttackerRemainingUnits.Count == 0
                ? 0
                : AttackerRemainingUnits.Average(u => u.BomberUnits.Count)
        );
    public UnitsStats DefenderRemainingUnitsAvg =>
        new(
            DefenderRemainingUnits.Count == 0
                ? 0
                : DefenderRemainingUnits.Average(u => u.InfantryUnits.Count),
            DefenderRemainingUnits.Count == 0
                ? 0
                : DefenderRemainingUnits.Average(u => u.ArtilleryUnits.Count),
            DefenderRemainingUnits.Count == 0
                ? 0
                : DefenderRemainingUnits.Average(u => u.TankUnits.Count),
            DefenderRemainingUnits.Count == 0
                ? 0
                : DefenderRemainingUnits.Average(u => u.FighterUnits.Count),
            DefenderRemainingUnits.Count == 0
                ? 0
                : DefenderRemainingUnits.Average(u => u.BomberUnits.Count)
        );
    private List<double> AttackerCpLoss { get; set; } = new List<double>();
    private List<double> DefenderCpLoss { get; set; } = new List<double>();
    public double AttackerAvgCpLoss => AttackerCpLoss.Count == 0 ? 0 : AttackerCpLoss.Average();
    public double DefenderAvgCpLoss => DefenderCpLoss.Count == 0 ? 0 : DefenderCpLoss.Average();

    public SimulationStats()
    {
        AttackerWon = 0;
        DefenderWon = 0;
        Draw = 0;
    }

    public void RecordResult(BattleInfo info)
    {
        switch (info.Result)
        {
            case BattleResult.AttackerVictory:
                AttackerWon++;
                break;
            case BattleResult.DefenderVictory:
                DefenderWon++;
                break;
            case BattleResult.Draw:
                Draw++;
                break;
        }
        AttackerRemainingUnits.Add(info.AttackerRemainingUnits);
        AttackerCpLoss.Add(info.AttackingArmy.Cost - info.AttackerRemainingUnits.Cost);

        DefenderRemainingUnits.Add(info.DefenderRemainingUnits);
        DefenderCpLoss.Add(info.DefendingArmy.Cost - info.DefenderRemainingUnits.Cost);
    }

    public override string ToString()
    {
        return $"Battle Results:\nAttacker Wins: {AttackerWon}, {AttackerWonPercentage:F2}%\nDefender Wins: {DefenderWon}, {DefenderWonPercentage:F2}%\nDraws: {Draw}, {DrawPercentage:F2}%";
    }

    public void Explain()
    {
        Console.WriteLine(ToString());
        Console.WriteLine($"Average Attacker Remaining Units:\n {AttackerRemainingUnitsAvg}");
        Console.WriteLine($"Average Defender Remaining Units:\n {DefenderRemainingUnitsAvg}");
        Console.WriteLine($"Average Attacker CP Loss: {AttackerAvgCpLoss:F2}");
        Console.WriteLine($"Average Defender CP Loss: {DefenderAvgCpLoss:F2}");
    }
}
