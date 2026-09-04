using axis_console_project.Resolvers;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

var army = new LandArmy(true,5,5,1,0,0);

var builder = new LandArmyBuilder();
var solution = builder.CreateCounterArmy(army, sims: 2000, cost: 35);
Console.WriteLine(solution);
