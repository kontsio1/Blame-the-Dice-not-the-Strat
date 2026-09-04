using axis_console_project.Armies;
using axis_console_project.Simulations;

namespace axis_console_project.Resolvers;

public abstract class ArmyBuilder
{
    public double ProgressPercentage { get; private set; }

    public Army CreateCounterArmy(Army targetArmy, int? cost = null, int sims = 1000, Func<SimulationStats, double>? propertySelectorForCounter = null, Selection selectionStrategy = Selection.Maximise, bool verbose = true, Action<double>? progressCallback = null, CancellationToken cancellationToken = default)
    {
        cost ??= targetArmy.Cost;
        propertySelectorForCounter ??= stats => stats.WonPercentage(!targetArmy.IsAttacking);

        SimulationStats? bestResult = null;
        ProgressPercentage = 0;

        List<Army> candidateArmies = CreateArmiesFromCost((int)cost, !targetArmy.IsAttacking).ToList();
        if (candidateArmies.Count == 0) throw new Exception("Couldn't create army. Try increasing the cost");

        for (int i = 0; i < candidateArmies.Count; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var simulation = new Simulation(candidateArmies[i], targetArmy);
            simulation.Run(sims, cancellationToken: cancellationToken);

            bestResult = UpdateBestResult(bestResult, simulation.Stats, propertySelectorForCounter, selectionStrategy);

            ProgressPercentage = (i + 1) * 100.0 / candidateArmies.Count;
            progressCallback?.Invoke(ProgressPercentage);
            Console.Write($"\r--- {ProgressPercentage:F2}% Complete ---");
        }

        ProgressPercentage = 100;
        progressCallback?.Invoke(ProgressPercentage);

        if (verbose)
        {
            bestResult?.Explain();
        }

        var finalResult = bestResult ?? throw new InvalidOperationException("Couldn't determine a best counter army.");

        if (targetArmy.IsAttacking) return finalResult.DefendingArmy;
        return finalResult.AttackingArmy;
    }

    private SimulationStats UpdateBestResult(SimulationStats? bestResult, SimulationStats currentStats, Func<SimulationStats, double> propertySelector, Selection selection)
    {
        if (bestResult is null) return currentStats;
        if (selection == Selection.Maximise)
        {
            return propertySelector(currentStats) > propertySelector(bestResult) ? currentStats : bestResult;
        }
        if(selection == Selection.Minimise)
        {
            return propertySelector(currentStats) < propertySelector(bestResult) ? currentStats : bestResult;
        }
        return bestResult;
    }

    public abstract IEnumerable<Army> CreateArmiesFromCost(int maxCost, bool isAttacking = true);
}

public enum Selection
{
    Minimise,
    Maximise
}