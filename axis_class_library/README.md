# Axis & Allies Battle Simulator

A battle simulator to get you justice on unlucky outcomes for the **Axis & Allies 1942** board game. Run thousands of simulations to understand the odds, find optimal army compositions, and prove your dice were truly cursed!

## Installation

Install the NuGet package:

```bash
dotnet add package AxisAndAlliesBattleSimulator
```

## Quick Start

```csharp
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using axis_console_project.Resolvers;

// Create a simple land battle simulation
var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3, artilleryCount: 1, tankCount: 2);
var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5, artilleryCount: 1);

var simulation = new Simulation(attackingArmy, defendingArmy);
simulation.Run(numberOfSimulations: 10000);

// Display detailed results
simulation.Stats.Explain();
```

## Features

- **Battle Simulation**: Run single or thousands of battle simulations
- **Land Battles**: Infantry, Artillery, Tanks, Fighters, Bombers, Anti-Air support
- **Naval Battles**: Submarines, Destroyers, Cruisers, Battleships, Aircraft Carriers, Transports
- **Optimal Army Builder**: Find the best counter-army for any opponent within a budget
- **Detailed Statistics**: Win percentages, average CP losses, remaining units

---

## Army Types

### Land Army

Create a land army with various unit combinations:

```csharp
var army = new LandArmy(
    isAttacking: true,        // true = attacker, false = defender
    infantryCount: 5,         // Cost: 3 IPCs | Attack: 1 (2 with artillery) | Defense: 2
    artilleryCount: 2,        // Cost: 4 IPCs | Attack: 2 | Defense: 2
    tankCount: 3,             // Cost: 5 IPCs | Attack: 3 | Defense: 3
    fighterCount: 1,          // Cost: 10 IPCs | Attack: 3 | Defense: 4
    bomberCount: 0,           // Cost: 12 IPCs | Attack: 4 | Defense: 1
    antiAirCount: 0,          // Cost: 6 IPCs | Special: Fires at aircraft
    cruiserCount: 0,          // Cost: 12 IPCs | Can bombard land units
    battleshipCount: 0        // Cost: 20 IPCs | Can bombard land units, 2 HP
);
```

### Naval Armada

Create a naval fleet for sea battles:

```csharp
using axis_console_project.UnitTypes.Sea;

var fleet = new NavalArmada(
    isAttacking: true,        // true = attacker, false = defender
    transportCount: 2,        // Cost: 7 IPCs | Attack: 0 | Defense: 0
    submarineCount: 3,        // Cost: 6 IPCs | Attack: 2 | Defense: 1
    destroyerCount: 1,        // Cost: 8 IPCs | Attack: 2 | Defense: 2
    cruiserCount: 1,          // Cost: 12 IPCs | Attack: 3 | Defense: 3
    battleshipCount: 1,       // Cost: 20 IPCs | Attack: 4 | Defense: 4, 2 HP
    carrierCount: 1,          // Cost: 14 IPCs | Attack: 1 | Defense: 2
    fighterCount: 2,          // Cost: 10 IPCs | Attack: 3 | Defense: 4
    bomberCount: 0            // Cost: 12 IPCs | Attack: 4 | Defense: 1
);
```

---

## Running Simulations

### Single Battle

Run one battle and see the result:

```csharp
var simulation = new Simulation(attackingArmy, defendingArmy);
var result = simulation.RunOnce();
```

### Multiple Simulations

Run thousands of simulations for statistical accuracy:

```csharp
var simulation = new Simulation(attackingArmy, defendingArmy);
simulation.Run(numberOfSimulations: 20000);

// Access statistics
Console.WriteLine($"Attacker Win Rate: {simulation.Stats.AttackerWonPercentage:F2}%");
Console.WriteLine($"Defender Win Rate: {simulation.Stats.DefenderWonPercentage:F2}%");
Console.WriteLine($"Draw Rate: {simulation.Stats.DrawPercentage:F2}%");

// Full summary
simulation.Stats.Explain();
```

### Simulation Statistics

The `SimulationStats` class provides detailed metrics:

| Property | Description |
|----------|-------------|
| `AttackerWonPercentage` | Percentage of battles won by attacker |
| `DefenderWonPercentage` | Percentage of battles won by defender |
| `DrawPercentage` | Percentage of battles ending in a draw |
| `AttackerAvgCpLoss` | Average IPC value of units lost by attacker |
| `DefenderAvgCpLoss` | Average IPC value of units lost by defender |
| `AttackerRemainingUnitsAvg` | Average surviving units for attacker |
| `DefenderRemainingUnitsAvg` | Average surviving units for defender |
| `TotalBattles` | Total number of simulations run |

---

## Finding Optimal Army Compositions

The army builders help you find the best army to counter an opponent within a budget.

### Land Army Builder

```csharp
using axis_console_project.Resolvers;

var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 7, artilleryCount: 1, tankCount: 1);
var armyBuilder = new LandArmyBuilder();

// Find the best attacking army with 39 IPCs that maximizes win percentage
var optimalArmy = armyBuilder.CreateCounterArmy(
    targetArmy: defendingArmy,
    cost: 39,
    propertySelectorForCounter: stats => stats.WonPercentage(),
    selectionStrategy: Selection.Maximise
);
```

### Naval Armada Builder

```csharp
var enemyFleet = new NavalArmada(isAttacking: false, submarineCount: 2, destroyerCount: 2, cruiserCount: 1);
var navalBuilder = new NavalArmadaBuilder();

// Find the best attacking fleet with 50 IPCs
var optimalFleet = navalBuilder.CreateCounterArmy(
    targetArmy: enemyFleet,
    cost: 50,
    propertySelectorForCounter: stats => stats.WonPercentage(),
    selectionStrategy: Selection.Maximise
);
```

### Selection Strategies

Use `Selection` enum to optimize for different goals:

```csharp
// Maximize win percentage (best chance to win)
selectionStrategy: Selection.Maximise,
propertySelectorForCounter: stats => stats.WonPercentage()

// Minimize IPC losses (most cost-efficient)
selectionStrategy: Selection.Minimise,
propertySelectorForCounter: stats => stats.AvgCpLoss()
```

### CreateCounterArmy Parameters

| Parameter | Description |
|-----------|-------------|
| `targetArmy` | The enemy army to counter |
| `cost` | Maximum IPC budget (defaults to enemy army cost) |
| `sims` | Number of simulations per composition (default: 1000) |
| `propertySelectorForCounter` | Function to extract the metric to optimize |
| `selectionStrategy` | `Selection.Maximise` or `Selection.Minimise` |
| `verbose` | Print results to console (default: true) |

---

## Exploring Army Combinations

Generate and evaluate all possible army combinations within a budget:

```csharp
var armyBuilder = new LandArmyBuilder();
var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5);

// Generate all attacking armies up to 24 IPCs
IEnumerable<Army> armies = armyBuilder.CreateArmiesFromCost(maxCost: 24, isAttacking: true);

var results = new List<SimulationStats>();

foreach (var army in armies)
{
    var simulation = new Simulation(army, defendingArmy);
    simulation.Run(1000);
    results.Add(simulation.Stats);
}

// Sort by win percentage
var ranked = results.OrderByDescending(r => r.AttackerWonPercentage).ToList();

// Show top 3 compositions
foreach (var stats in ranked.Take(3))
{
    stats.Explain();
}
```

---

## Unit Reference

### Land Units

| Unit | Cost | Attack | Defense | Notes |
|------|------|--------|---------|-------|
| Infantry | 3 | 1 (2*) | 2 | *Attack 2 when paired with artillery |
| Artillery | 4 | 2 | 2 | Boosts paired infantry attack |
| Tank | 5 | 3 | 3 | |
| Anti-Air | 6 | 1 | 0 | Fires at aircraft before combat |

### Air Units

| Unit | Cost | Attack | Defense | Notes |
|------|------|--------|---------|-------|
| Fighter | 10 | 3 | 4 | |
| Bomber | 12 | 4 | 1 | |

### Naval Units

| Unit | Cost | Attack | Defense | Notes |
|------|------|--------|---------|-------|
| Transport | 7 | 0 | 0 | |
| Submarine | 6 | 2 | 1 | |
| Destroyer | 8 | 2 | 2 | |
| Cruiser | 12 | 3 | 3 | Can bombard land |
| Aircraft Carrier | 14 | 1 | 2 | Carries 2 fighters |
| Battleship | 20 | 4 | 4 | Can bombard land, 2 HP |

---

## Example: Complete Workflow

```csharp
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using axis_console_project.Resolvers;

// 1. Define the battle scenario
var defendingArmy = new LandArmy(
    isAttacking: false,
    infantryCount: 7,
    artilleryCount: 1,
    tankCount: 1,
    antiAirCount: 0
);

// 2. Find optimal counter with 39 IPCs
var armyBuilder = new LandArmyBuilder();
var optimalAttacker = armyBuilder.CreateCounterArmy(
    targetArmy: defendingArmy,
    cost: 39,
    propertySelectorForCounter: s => s.WonPercentage(),
    selectionStrategy: Selection.Maximise
);

// 3. Run additional simulations for verification
var verification = new Simulation(optimalAttacker, defendingArmy);
verification.Run(50000);
verification.Stats.Explain();
```

---

## License

See [LICENSE](LICENSE) file for details.
