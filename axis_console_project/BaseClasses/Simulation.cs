using axis_console_project.UnitTypes;

namespace axis_console_project.BaseClasses;

public class Simulation(Army attackingArmy, Army defendingArmy, int numberOfSimulations)
{
    public SimulationStats Stats { get; set; } = new();
    public Army AttackingArmy { get; } = attackingArmy;
    public Army DefendingArmy { get; } = defendingArmy;
    public int NumberOfSimulations { get; set; } = numberOfSimulations;

    public override string ToString()
    {
        return $"\nSimulation for armies:\n{AttackingArmy.units} \nvs\n {DefendingArmy.units} \nrunning {NumberOfSimulations} simulations.\n";
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
            Console.Write($"\r--- {(double)i / NumberOfSimulations * 100:F2}% Complete ---");
            var battle = new Battle(AttackingArmy.Clone(), DefendingArmy.Clone());
            var battleResult = battle.Fight();
            Stats.RecordResult(battleResult);
        }

        return Stats;
    }
}

public class SimulationStats
{
    public Army AttackingArmy { get; set; }
    public Army DefendingArmy { get; set; }
    public int AttackerWon { get; set; } = 0;
    public int DefenderWon { get; set; } = 0;
    public int Draw { get; set; } = 0;
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
            GetAverageUnits(AttackerRemainingUnits, u => u.InfantryUnits),
            GetAverageUnits(AttackerRemainingUnits, u => u.ArtilleryUnits),
            GetAverageUnits(AttackerRemainingUnits, u => u.TankUnits),
            GetAverageUnits(AttackerRemainingUnits, u => u.FighterUnits),
            GetAverageUnits(AttackerRemainingUnits, u => u.BomberUnits)
        );
    public UnitsStats DefenderRemainingUnitsAvg =>
        new UnitsStats(
            GetAverageUnits(DefenderRemainingUnits, u => u.InfantryUnits),
            GetAverageUnits(DefenderRemainingUnits, u => u.ArtilleryUnits),
            GetAverageUnits(DefenderRemainingUnits, u => u.TankUnits),
            GetAverageUnits(DefenderRemainingUnits, u => u.FighterUnits),
            GetAverageUnits(DefenderRemainingUnits, u => u.BomberUnits)
        );

    private double GetAverageUnits(
        List<Units> unitsList,
        Func<Units, IEnumerable<Unit>> unitSelector
    )
    {
        return unitsList.Count == 0
            ? 0
            : unitsList.Average(u =>
                unitSelector(u).Where(unit => unit.ParticipatesInBattle).ToList().Count
            );
    }

    private List<double> AttackerCpLoss { get; set; } = new List<double>();
    private List<double> DefenderCpLoss { get; set; } = new List<double>();
    public double AttackerAvgCpLoss => AttackerCpLoss.Count == 0 ? 0 : AttackerCpLoss.Average();
    public double DefenderAvgCpLoss => DefenderCpLoss.Count == 0 ? 0 : DefenderCpLoss.Average();

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
        AttackerCpLoss.Add(
            info.AttackingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - info.AttackerRemainingUnits.Cost
        );

        DefenderRemainingUnits.Add(info.DefenderRemainingUnits);
        DefenderCpLoss.Add(
            info.DefendingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - info.DefenderRemainingUnits.Cost
        );
    }

    public void Explain()
    {
        Console.WriteLine("\n--- Simulation Summary ---\n");
        Console.WriteLine(
            $"Battle Results:\nAttacker Wins: {AttackerWon}, {AttackerWonPercentage:F2}%\nDefender Wins: {DefenderWon}, {DefenderWonPercentage:F2}%\nDraws: {Draw}, {DrawPercentage:F2}%"
        );
        Console.WriteLine(
            $"Attacker Army:\n{AttackingArmy.units} \nDefending Army:\n{DefendingArmy.units}"
        );
        Console.WriteLine($"Average Attacker Remaining Units:\n {AttackerRemainingUnitsAvg}");
        Console.WriteLine($"Average Defender Remaining Units:\n {DefenderRemainingUnitsAvg}");
        Console.WriteLine($"Average Attacker CP Loss: {AttackerAvgCpLoss:F2}");
        Console.WriteLine($"Average Defender CP Loss: {DefenderAvgCpLoss:F2}");
        Console.WriteLine($"{AttackingArmy.Cost} CP vs {DefendingArmy.Cost} CP");
    }
}
