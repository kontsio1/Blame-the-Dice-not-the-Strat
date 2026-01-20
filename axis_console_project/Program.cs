// See https://aka.ms/new-console-template for more information
using axis_console_project.BaseClasses;
using axis_console_project.Resolvers;

Console.WriteLine("Hello, World!");

var totalSims = 10000;

// var simulation = new Simulation(
//     new Army(true, 1, 1, 1, 0, 1, CruiserCount: 0, BattleshipCount: 10),
//     new Army(false, 4, 0, 0, 0),
//     totalSims
// );
// var simResults = simulation.Run();
// Console.WriteLine(simulation.ToString());
// simResults.Explain();
// Console.WriteLine($"{simulation.AttackingArmy.Cost} CP vs {simulation.DefendingArmy.Cost} CP");
var defendingArmy = new Army(false, 5, 0, 0, 0);
var armies = ArmyCompResolver.GetPossibleArmies(25, true);

var optimalCompResults = armies
    .Select(attackingArmy =>
    {
        var simulation = new Simulation(attackingArmy, defendingArmy, totalSims);
        var result = simulation.Run();
        return result;
    })
    .OrderByDescending(r => r.AttackerWonPercentage)
    .Take(3)
    .ToList();

foreach (SimulationStats sim in optimalCompResults)
{
    Console.WriteLine("-------------------------------");
    sim.Explain();
}
