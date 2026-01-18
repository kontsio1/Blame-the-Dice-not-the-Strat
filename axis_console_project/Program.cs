// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var totalSims = 10000;

var simulation = new Simulation(
    new Army(true, 1, 1, 1, 0, 1, CruiserCount: 0, BattleshipCount: 10),
    new Army(false, 4, 0, 0, 0),
    totalSims
);
var simResults = simulation.Run();
Console.WriteLine(simulation.ToString());
simResults.Explain();
Console.WriteLine($"{simulation.AttackingArmy.Cost} CP vs {simulation.DefendingArmy.Cost} CP");

// var comps = Helpers.GetAllCombinations(20);
