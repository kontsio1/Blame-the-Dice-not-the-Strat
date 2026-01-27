// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using axis_console_project.BaseClasses;
using axis_console_project.Resolvers;
using axis_console_project.UnitTypes.Land;

Console.WriteLine("Hello, World!");

var battleSims = 20000;

// var simulation = new Simulation(
//     new LandArmy(true, 1, 1, 1, fighterCount: 10),
//     new LandArmy(false, 15, fighterCount: 1, antiAirCount: 0),
//     battleSims
// );
// var simResults = simulation.Run();
// simResults.Explain();

var defendingArmy = new LandArmy(false, 5, 0, 0, 0);
IEnumerable<Army>? armies = ArmyCompResolver.GetPossibleArmies(24, true).Where(c => c.Cost > 23);
var i = 0;

var totalSims = armies.Count();
Console.WriteLine($"\nEvaluating Army Composition");
var compBattleResults = armies
    .Select(attackingArmy =>
    {
        i++;
        Console.Write($"\r--- {(double)i / totalSims * 100:F2}% Complete ---");
        var simulation = new Simulation(attackingArmy, defendingArmy, totalSims);
        var result = simulation.Run();
        return result;
    })
    .OrderByDescending(r => r.AttackerWonPercentage);

List<SimulationStats> optimalCompResults =
[
    .. compBattleResults.Take(3),
    compBattleResults.Skip(armies.Count() / 2).First(),
    compBattleResults.Last(),
];

foreach (SimulationStats sim in optimalCompResults)
{
    Console.WriteLine("-------------------------------");
    sim.Explain();
}
