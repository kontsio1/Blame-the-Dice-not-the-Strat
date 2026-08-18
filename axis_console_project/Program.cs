// See https://aka.ms/new-console-template for more information

using System.Collections.Generic;
using axis_console_project.Armies;
using axis_console_project.Resolvers;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

Console.WriteLine("Hello, World!");

var battleSims = 20000;

// var simulation = new Simulation(
//     new LandArmy(true, 1, 1, 1, fighterCount: 10),
//     new LandArmy(false, 15, fighterCount: 1, antiAirCount: 0)
// );
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();
// simulation.RunOnce();

//-----
var defendingArmy = new LandArmy(false, 7, 1, 1, 0);
var armyBuilder = new ArmyBuilder();

Console.WriteLine(armyBuilder.CreateCounterArmy(defendingArmy, 39));

// IEnumerable<Army> armies = armyBuilder.CreateArmiesFromCost(24, true).Where(c => c.Cost > 23);
//
// var totalSims = armies.Count();
// Console.WriteLine($"\nEvaluating Army Composition");
//
// var compBattleResults = new List<SimulationStats>();
//
// for (int i = 0; i < totalSims; i++) 
// {
//         Console.Write($"\r--- {(double)i / totalSims * 100:F2}% Complete ---");
//         var simulation = new Simulation(armies.ElementAt(i), defendingArmy);
//         simulation.Run(100);
//         compBattleResults.Add(simulation.Stats);
//     
// }
//
// var compBattleResultsOrdered = compBattleResults.OrderByDescending(r => r.AttackerWonPercentage).ToList();
//
// List<SimulationStats> optimalCompResults =
// [
//     .. compBattleResultsOrdered.Take(3),
//     compBattleResultsOrdered.Skip(armies.Count() / 2).First(),
//     compBattleResultsOrdered.Last(),
// ];
// //SimulationSuite
//
// foreach (SimulationStats sim in optimalCompResults)
// {
//     Console.WriteLine("-------------------------------");
//     sim.Explain();
// }