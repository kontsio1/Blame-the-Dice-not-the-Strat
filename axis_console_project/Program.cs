// See https://aka.ms/new-console-template for more information
using axis_console_project.BaseClasses;
using axis_console_project.Resolvers;
using axis_console_project.UnitTypes.Land;

Console.WriteLine("Hello, World!");

var totalSims = 10000;

var simulation = new Simulation(
    new LandArmy(true, 1, 1, 1, 0, 1, antiAirCount: 0, cruiserCount: 0, battleshipCount: 3),
    new LandArmy(false, 5, 0, 0, 0),
    totalSims
);
var simResults = simulation.Run();
Console.WriteLine(simulation.ToString());
simResults.Explain();
Console.WriteLine($"{simulation.AttackingArmy.Cost} CP vs {simulation.DefendingArmy.Cost} CP");

// var defendingArmy = new Army(false, 5, 0, 0, 0);
// IEnumerable<Army>? armies = ArmyCompResolver.GetPossibleArmies(25, true);
// var i = 0;
// var numberOfSimulations = armies.Count();
// Console.WriteLine($"\nEvaluating Army Composition");
// var optimalCompResults = armies
//     .Select(attackingArmy =>
//     {
//         i++;
//         Console.Write($"\r--- {(double)i / numberOfSimulations * 100:F2}% Complete ---");
//         var simulation = new Simulation(attackingArmy, defendingArmy, totalSims);
//         var result = simulation.Run();
//         return result;
//     })
//     .OrderByDescending(r => r.AttackerWonPercentage)
//     .Take(3)
//     .ToList();

// foreach (SimulationStats sim in optimalCompResults)
// {
//     Console.WriteLine("-------------------------------");
//     sim.Explain();
// }
