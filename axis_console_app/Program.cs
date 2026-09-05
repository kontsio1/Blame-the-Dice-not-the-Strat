using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

var army = new LandArmy(true,4,2,2,0,0);
var army2 = new LandArmy(false,5,0,2,0,0);

var sim = new Simulation(army,army2);
sim.Run(10000);
sim.Stats.Explain();

var battleResult = sim.RunOnce();
// sim.Stats.CreateProbabilityDistribution();
var luckystats = sim.Stats.HowLuckyWasThisOutcome(battleResult);
luckystats.Explain();

// var builder = new LandArmyBuilder();
// var solution = builder.CreateCounterArmy(army, sims: 2000, cost: 35);
// Console.WriteLine(solution);
