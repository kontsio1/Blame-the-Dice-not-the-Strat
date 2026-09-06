// SimulationStats.cs

using axis_console_project.Armies;
using axis_console_project.Battles;

namespace axis_console_project.Simulations;

public class SimulationStats(Army? attackingArmy = null, Army? defendingArmy = null)
{
    public Army? AttackingArmy { get; set; } = attackingArmy;
    public Army? DefendingArmy { get; set; } = defendingArmy;
    public int AttackerWon { get; set; } = 0;
    public int DefenderWon { get; set; } = 0;
    public int Draw { get; set; } = 0;
    public int TotalBattles => AttackerWon + DefenderWon + Draw;

    public double AttackerWonPercentage =>
        TotalBattles == 0 ? 0 : (AttackerWon * 100.0) / TotalBattles;

    public double DefenderWonPercentage =>
        TotalBattles == 0 ? 0 : (DefenderWon * 100.0) / TotalBattles;

    public double DrawPercentage => TotalBattles == 0 ? 0 : (Draw * 100.0) / TotalBattles;
    public List<BattleResult> BattleResults { get; set; } = new List<BattleResult>();
    private List<double> AttackerCpLoss { get; set; } = new List<double>();
    private List<double> DefenderCpLoss { get; set; } = new List<double>();
    public double AttackerAvgCpLoss => AttackerCpLoss.Count == 0 ? 0 : AttackerCpLoss.Average();
    public double DefenderAvgCpLoss => DefenderCpLoss.Count == 0 ? 0 : DefenderCpLoss.Average();
    public double WonPercentage(bool forAttacker = true) => forAttacker ? AttackerWonPercentage : DefenderWonPercentage;
    public double AvgCpLoss(bool forAttacker = true) => forAttacker ? AttackerAvgCpLoss : DefenderAvgCpLoss;

    public double RemainingUnitsAvg(bool forAttacker = true) =>
        forAttacker
            ? BattleResults.Select(result => result.AttackerRemainingUnits).Count()
            : BattleResults.Select(result => result.DefenderRemainingUnits).Count();

    public UnitsStats AttackerRemainingUnitsAvg =>
        new UnitsStats(
            infantryUnits: GetAverageUnits(
                BattleResults,
                result => result.AttackerRemainingUnits,
                units => units.InfantryUnits
            ),
            artilleryUnits: GetAverageUnits(
                BattleResults,
                result => result.AttackerRemainingUnits,
                units => units.ArtilleryUnits
            ),
            tankUnits: GetAverageUnits(
                BattleResults,
                result => result.AttackerRemainingUnits,
                units => units.TankUnits
            ),
            fighterUnits: GetAverageUnits(
                BattleResults,
                result => result.AttackerRemainingUnits,
                units => units.FighterUnits
            ),
            bomberUnits: GetAverageUnits(
                BattleResults,
                result => result.AttackerRemainingUnits,
                units => units.BomberUnits
            )
        );

    public UnitsStats DefenderRemainingUnitsAvg =>
        new UnitsStats(
            infantryUnits: GetAverageUnits(
                BattleResults,
                result => result.DefenderRemainingUnits,
                units => units.InfantryUnits
            ),
            artilleryUnits: GetAverageUnits(
                BattleResults,
                result => result.DefenderRemainingUnits,
                units => units.ArtilleryUnits
            ),
            tankUnits: GetAverageUnits(
                BattleResults,
                result => result.DefenderRemainingUnits,
                units => units.TankUnits
            ),
            fighterUnits: GetAverageUnits(
                BattleResults,
                result => result.DefenderRemainingUnits,
                units => units.FighterUnits
            ),
            bomberUnits: GetAverageUnits(
                BattleResults,
                result => result.DefenderRemainingUnits,
                units => units.BomberUnits
            )
        );

    private double GetAverageUnits(
        List<BattleResult> battleResults,
        Func<BattleResult, Units> sideSelector,
        Func<Units, IEnumerable<Unit>> unitSelector
    )
    {
        return battleResults.Count == 0
            ? 0
            : battleResults.Average(result =>
                unitSelector(sideSelector(result)).Count(unit => unit.ParticipatesInBattle)
            );
    }

    public void RecordResult(BattleResult result)
    {
        ArgumentNullException.ThrowIfNull(AttackingArmy);
        ArgumentNullException.ThrowIfNull(DefendingArmy);

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

        BattleResults.Add(result);
        AttackerCpLoss.Add(
            AttackingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - result.AttackerRemainingUnits.Cost
        );

        DefenderCpLoss.Add(
            DefendingArmy.GetAllUnits()
                .Where(unit => unit.ParticipatesInBattle)
                .Sum(unit => unit.Cost) - result.DefenderRemainingUnits.Cost
        );
    }

    public List<(int Value, int Count, double Probability)> CreateProbabilityDistribution()
    {
        // 1. Get total count as a double to avoid integer division issues
        double totalSamples = BattleResults.Count;

        // 2. Group by value and calculate the percentage distribution
        var probabilityDistribution = BattleResults
            .GroupBy(x => x.AttackerRemainingUnits.Cost - x.DefenderRemainingUnits.Cost)
            .Select(group =>
            (
                Value: group.Key,
                Count: group.Count(),
                Probability: group.Count() / totalSamples
            ))
            .OrderByDescending(x => x.Value)
            .ToList();

        // Output the results
        foreach (var item in probabilityDistribution)
        {
            Console.WriteLine($"Value: {item.Value} | Count: {item.Count} | Probability: {item.Probability:P2}");
        }
        return probabilityDistribution;
    }

    public LuckyStats HowLuckyWasThisOutcome(Units attackerRemainingUnits, Units defenderRemainingUnits)
    {
        var outcome = DetermineOutcome(attackerRemainingUnits, defenderRemainingUnits);

        var result = new BattleResult(
            outcome,
            attackerRemainingUnits.GetAllUnits(),
            defenderRemainingUnits.GetAllUnits()
        );

        return HowLuckyWasThisOutcome(result);
    }

    public LuckyStats HowLuckyWasThisOutcome(BattleResult result)
    {
        ArgumentNullException.ThrowIfNull(AttackingArmy);
        ArgumentNullException.ThrowIfNull(DefendingArmy);

        var distribution = CreateProbabilityDistribution();
        var ipcAttackerLuck = result.AttackerRemainingUnits.Cost - (AttackingArmy.Cost - AttackerAvgCpLoss); // actual remaining icp - avg remaining icp
        var ipcDefenderLuck = result.DefenderRemainingUnits.Cost - (DefendingArmy.Cost - DefenderAvgCpLoss);

        var resultValue = result.AttackerRemainingUnits.Cost - result.DefenderRemainingUnits.Cost;

        // Percentile is the total probability mass of strictly worse outcomes.
        var percentile = distribution
            .Where(outcome => outcome.Value < resultValue)
            .Sum(outcome => outcome.Probability);

        var exactProbability = distribution
            .Where(outcome => outcome.Value == resultValue)
            .Select(outcome => outcome.Probability)
            .FirstOrDefault();

        // Shock value in bits of surprise: -log2(P).
        var shockValue = exactProbability > 0 ? -Math.Log2(exactProbability) : double.PositiveInfinity;
        
        return new LuckyStats
        {
            ipcAttackerLuck = ipcAttackerLuck,
            ipcDefenderLuck = ipcDefenderLuck,
            percentile = percentile,
            shock = shockValue
        };
    }

    private static BattleOutcome DetermineOutcome(Units attackerRemainingUnits, Units defenderRemainingUnits)
    {
        var attackerHasUnits = attackerRemainingUnits.GetAllUnits().Any(unit => unit.ParticipatesInBattle);
        var defenderHasUnits = defenderRemainingUnits.GetAllUnits().Any(unit => unit.ParticipatesInBattle);

        if (attackerHasUnits && defenderHasUnits)
        {
            throw new InvalidOperationException(
                "The actual result must represent a completed battle. Only one side, or neither side in a draw, can have remaining participating units."
            );
        }

        if (attackerHasUnits)
        {
            return BattleOutcome.AttackerVictory;
        }

        if (defenderHasUnits)
        {
            return BattleOutcome.DefenderVictory;
        }

        return BattleOutcome.Draw;
    }

    public void Explain()
    {
        ArgumentNullException.ThrowIfNull(AttackingArmy);
        ArgumentNullException.ThrowIfNull(DefendingArmy);

        Console.WriteLine("\n--- Simulation Summary ---\n");
        Console.WriteLine(
            $"Battle Results:\nAttacker Wins: {AttackerWon}, {AttackerWonPercentage:F2}%\nDefender Wins: {DefenderWon}, {DefenderWonPercentage:F2}%\nDraws: {Draw}, {DrawPercentage:F2}%"
        );
        Console.WriteLine(
            $"Attacker Army:\n{AttackingArmy.Units} \nDefending Army:\n{DefendingArmy.Units}"
        );
        Console.WriteLine($"Average Attacker Remaining Units:\n {AttackerRemainingUnitsAvg}");
        Console.WriteLine($"Average Defender Remaining Units:\n {DefenderRemainingUnitsAvg}");
        Console.WriteLine($"Average Attacker IPC Loss: {AttackerAvgCpLoss:F2}");
        Console.WriteLine($"Average Defender IPC Loss: {DefenderAvgCpLoss:F2}");
        Console.WriteLine($"{AttackingArmy.Cost} IPC vs {DefendingArmy.Cost} IPC");
    }
}

public class LuckyStats
{
    public double ipcAttackerLuck { get; set; }
    public double ipcDefenderLuck { get; set; }
    public double percentile { get; set; }
    public double shock { get; set; }
    
    public void Explain()
    {
        Console.WriteLine($"IPC Attacker Luck: {ipcAttackerLuck:F2}");
        Console.WriteLine($"IPC Defender Luck: {ipcDefenderLuck:F2}");
        Console.WriteLine($"Percentile (probability of worse outcomes): {percentile:P2}");
        Console.WriteLine($"Shock value: {shock:F2} bits");
    }
}