using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

var attackingArmy = new LandArmy(true,6,0,6,6,1);
var defendingArmy = new LandArmy(false, 7,1,3,2, antiAirCount:1);

var simulation = new Simulation(attackingArmy, defendingArmy);
simulation.Run(1000000);

simulation.Stats.Explain();
