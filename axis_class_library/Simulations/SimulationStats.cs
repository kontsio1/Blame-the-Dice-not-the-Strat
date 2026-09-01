// SimulationStats.cs

using axis_console_project.Armies;
using axis_console_project.Battles;

namespace axis_console_project.Simulations;

public class SimulationStats(Army? attackingArmy = null, Army? defendingArmy = null)
{
    public Army? AttackingArmy { get; set; } = attackingArmy;
    public Army? DefendingArmy { get; set; } =  defendingArmy;
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
    private List<double> AttackerCpLoss { get; set; } = new List<double>();
    private List<double> DefenderCpLoss { get; set; } = new List<double>();
    public double AttackerAvgCpLoss => AttackerCpLoss.Count == 0 ? 0 : AttackerCpLoss.Average();
    public double DefenderAvgCpLoss => DefenderCpLoss.Count == 0 ? 0 : DefenderCpLoss.Average();
    public double WonPercentage(bool forAttacker = true) => forAttacker ? AttackerWonPercentage : DefenderWonPercentage;
    public double AvgCpLoss(bool forAttacker = true) => forAttacker ? AttackerAvgCpLoss : DefenderAvgCpLoss;
    public double RemainingUnitsAvg(bool forAttacker = true) => forAttacker ? AttackerRemainingUnits.Count : DefenderRemainingUnits.Count;
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

    public void RecordResult(BattleResult result)
    {
        switch (result.BattleOutcome)
        {
            case BattleOutcome.AttackerVictory:
                AttackerWon++;
                break;
            case BattleOutcome.DefenderVictory:
                DefenderWon++;
                break;
            case BattleOutcome.Draw:
                Draw++;
                break;
        }
        AttackerRemainingUnits.Add(result.AttackerRemainingUnits);
        AttackerCpLoss.Add(
            result.AttackingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - result.AttackerRemainingUnits.Cost
        );

        DefenderRemainingUnits.Add(result.DefenderRemainingUnits);
        DefenderCpLoss.Add(
            result.DefendingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - result.DefenderRemainingUnits.Cost
        );
    }

    public void Explain()
    {
        Console.WriteLine("\n--- Simulation Summary ---\n");
        Console.WriteLine(
            $"Battle Results:\nAttacker Wins: {AttackerWon}, {AttackerWonPercentage:F2}%\nDefender Wins: {DefenderWon}, {DefenderWonPercentage:F2}%\nDraws: {Draw}, {DrawPercentage:F2}%"
        );
        Console.WriteLine(
            $"Attacker Army:\n{AttackingArmy.Units} \nDefending Army:\n{DefendingArmy.Units}"
        );
        Console.WriteLine($"Average Attacker Remaining Units:\n {AttackerRemainingUnitsAvg}");
        Console.WriteLine($"Average Defender Remaining Units:\n {DefenderRemainingUnitsAvg}");
        Console.WriteLine($"Average Attacker CP Loss: {AttackerAvgCpLoss:F2}");
        Console.WriteLine($"Average Defender CP Loss: {DefenderAvgCpLoss:F2}");
        Console.WriteLine($"{AttackingArmy.Cost} CP vs {DefendingArmy.Cost} CP");
    }
}