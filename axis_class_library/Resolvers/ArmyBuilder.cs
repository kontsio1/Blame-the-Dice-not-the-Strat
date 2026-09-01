using axis_console_project.Armies;
using axis_console_project.Simulations;

namespace axis_console_project.Resolvers;

public abstract class ArmyBuilder
{
    public Army CreateCounterArmy(Army targetArmy, int? cost = null, int sims = 1000, Func<SimulationStats, double>? propertySelectorForCounter = null, Selection selectionStrategy = Selection.Maximise, bool verbose = true)
    {
        cost ??= targetArmy.Cost;
        propertySelectorForCounter ??= stats => stats.WonPercentage(!targetArmy.IsAttacking);

        SimulationStats? bestResult = null;

        List<Army> candidateArmies = CreateArmiesFromCost((int)cost, !targetArmy.IsAttacking).ToList();
        if (candidateArmies.Count == 0) throw new Exception("Couldn't create army. Try increasing the cost");

        for (int i = 0; i < candidateArmies.Count; i++)
        {
            var simulation = new Simulation(candidateArmies[i], targetArmy);
            simulation.Run(sims);

            bestResult = UpdateBestResult(bestResult, simulation.Stats, propertySelectorForCounter, selectionStrategy);

            Console.Write($"\r--- {(double)i / candidateArmies.Count * 100:F2}% Complete ---");
        }

        if (verbose)
        {
            bestResult.Explain();
        }

        if (targetArmy.IsAttacking) return bestResult.DefendingArmy;
        return bestResult.AttackingArmy;
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