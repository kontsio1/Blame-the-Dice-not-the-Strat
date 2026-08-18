using axis_console_project.Armies;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.Resolvers;

public class ArmyBuilder
{
    private bool _isLandArmy { get; set; } = true;
    
    public bool IsLandArmy
    {
        get => _isLandArmy;
        set => _isLandArmy = value;
    }

    public bool IsNavalArmy
    {
        get => !_isLandArmy;
        set => _isLandArmy = !value;
    }

    public Army CreateCounterArmy(Army targetArmy, int? cost = null, int sims = 1000)
    {
        cost ??= targetArmy.Cost;
    
        SimulationStats bestResult = new();
    
        List<Army> candidateArmies = CreateArmiesFromCost((int)cost, !targetArmy.IsAttacking).ToList();
        if(candidateArmies.Count == 0) throw new Exception("Couldn't create army. Try increasing the cost");
        
        for (int i = 0; i < candidateArmies.Count; i++)
        {
            var simulation = new Simulation(candidateArmies[i], targetArmy);
            simulation.Run(sims);
            
            if (targetArmy.IsAttacking)
            {
                bestResult = bestResult.DefenderWonPercentage < simulation.Stats.DefenderWonPercentage ? simulation.Stats : bestResult;
            }
            if (targetArmy.IsDefending)
            {
                bestResult = bestResult.AttackerWonPercentage < simulation.Stats.AttackerWonPercentage ? simulation.Stats : bestResult;
            }

            Console.Write($"\r--- {(double)i / candidateArmies.Count * 100:F2}% Complete ---");
        }
        
        bestResult.Explain();
    
        if (targetArmy.IsAttacking) return bestResult.DefendingArmy;
        return bestResult.AttackingArmy;
    }
    
    public IEnumerable<Army> CreateArmiesFromCost(int maxCost, bool isAttacking = true)
    {
        //TODO: change to naval army based on battle type
        IEnumerable<Army> armies = Enumerable.Empty<Army>();

        if (IsLandArmy)
        {
            var armyComps = GetAllLandCombinations(maxCost);
            armies = armyComps.Select(c => new LandArmy(
                isAttacking,
                c.InfantryCount,
                c.ArtilleryCount,
                c.TankCount,
                c.FighterCount,
                c.BomberCount
            ));
        }
        if (IsNavalArmy)
        {
            var armyComps = GetAllLandCombinations(maxCost);
            armies = armyComps.Select(c => new NavalArmada(
                isAttacking,
                c.InfantryCount,
                c.ArtilleryCount,
                c.TankCount,
                c.FighterCount,
                c.BomberCount
            ));
        }
        return armies;
    }

    private List<LandArmyComp> GetAllLandCombinations(int cost)
    {
        var combinations = new List<LandArmyComp>();

        int maxInf = cost / Infantry.UnitCost;
        int maxArt = cost / Artillery.UnitCost;
        int maxTank = cost / Tank.UnitCost;
        int maxFighter = cost / Fighter.UnitCost;
        int maxBomber = cost / Bomber.UnitCost;

        var armyComp = new LandArmyComp(0, 0, 0, 0, 0);

        for (int inf = 0; inf <= maxInf; inf++)
        {
            armyComp.InfantryCount = inf;

            for (int art = 0; art <= maxArt; art++)
            {
                armyComp.ArtilleryCount = art;

                for (int tank = 0; tank <= maxTank; tank++)
                {
                    armyComp.TankCount = tank;

                    for (int fighter = 0; fighter <= maxFighter; fighter++)
                    {
                        armyComp.FighterCount = fighter;

                        for (int bomber = 0; bomber <= maxBomber; bomber++)
                        {
                            armyComp.BomberCount = bomber;
                            if (armyComp.Cost <= cost && armyComp.Cost != 0)
                            {
                                combinations.Add(armyComp.Clone());
                                // Console.WriteLine(armyComp);
                            }
                        }
                    }
                }
            }
        }

        return combinations;
    }
}

internal class LandArmyComp
{
    public LandArmyComp(
        int infantryCount,
        int artilleryCount,
        int tankCount,
        int fighterCount,
        int bomberCount
    )
    {
        InfantryCount = infantryCount;
        ArtilleryCount = artilleryCount;
        TankCount = tankCount;
        FighterCount = fighterCount;
        BomberCount = bomberCount;
    }
    public LandArmyComp(LandArmy army)
    {
        InfantryCount = army.Units.InfantryUnits.Count;
        ArtilleryCount = army.Units.ArtilleryUnits.Count;
        TankCount = army.Units.TankUnits.Count;
        FighterCount = army.Units.FighterUnits.Count;
        BomberCount = army.Units.BomberUnits.Count;
    }
    public int InfantryCount { get; set; }
    public int ArtilleryCount { get; set; }
    public int TankCount { get; set; }
    public int FighterCount { get; set; }
    public int BomberCount { get; set; }
    public int Cost =>
        InfantryCount * Infantry.UnitCost
        + ArtilleryCount * Artillery.UnitCost
        + TankCount * Tank.UnitCost
        + FighterCount * Fighter.UnitCost
        + BomberCount * Bomber.UnitCost;

    public override string ToString()
    {
        return $"[{InfantryCount},{ArtilleryCount},{TankCount},{FighterCount},{BomberCount}] Cost: {Cost}";
    }

    public LandArmyComp Clone()
    {
        return new LandArmyComp(InfantryCount, ArtilleryCount, TankCount, FighterCount, BomberCount);
    }
}

internal class NavalArmadaComp(
    int transportCount,
    int destroyerCount,
    int submarineCount,
    int cruiserCount,
    int battleshipCount,
    int fighterCount,
    int bomberCount
)
{
    public int TransportCount { get; set; } = transportCount;
    public int DestroyerCount { get; set; } = destroyerCount;
    public int SubmarineCount { get; set; } = submarineCount;
    public int CruiserCount { get; set; } = cruiserCount;
    public int BomberCount { get; set; } = bomberCount;
    public int Cost =>
        TransportCount * Transport.UnitCost
        + DestroyerCount * Destroyer.UnitCost
        + SubmarineCount * Submarine.UnitCost
        + CruiserCount * Cruiser.UnitCost
        + BomberCount * Bomber.UnitCost;

    public override string ToString()
    {
        return $"[{TransportCount},{DestroyerCount},{SubmarineCount},{CruiserCount},{BomberCount}] Cost: {Cost}";
    }

    public LandArmyComp Clone()
    {
        return new LandArmyComp(TransportCount, DestroyerCount, SubmarineCount, CruiserCount, BomberCount);
    }
}
