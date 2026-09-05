using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

var army = new LandArmy(true,0,0,1,0,0);
var army2 = new LandArmy(false,0,0,1,0,0);

var sim = new Simulation(army,army2);
sim.Run(1000);
sim.Stats.Explain();

// var builder = new LandArmyBuilder();
// var solution = builder.CreateCounterArmy(army, sims: 2000, cost: 35);
// Console.WriteLine(solution);
