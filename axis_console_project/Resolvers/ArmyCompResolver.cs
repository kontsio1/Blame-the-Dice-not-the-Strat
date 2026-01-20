using axis_console_project.BaseClasses;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;

namespace axis_console_project.Resolvers;

public static class ArmyCompResolver
{
    public static IEnumerable<Army> GetPossibleArmies(int maxCost, bool isAttacking = true)
    {
        var armyComps = GetAllCombinations(maxCost);
        var armies = armyComps.Select(c => new Army(
            isAttacking,
            c.InfantryCount,
            c.ArtilleryCount,
            c.TankCount,
            c.FighterCount,
            c.BomberCount
        ));
        return armies;
    }

    public static List<ArmyComp> GetAllCombinations(int cost)
    {
        var combinations = new List<ArmyComp>();

        int maxInf = cost / Infantry.cost;
        int maxArt = cost / Artillery.cost;
        int maxTank = cost / Tank.cost;
        int maxFighter = cost / Fighter.cost;
        int maxBomber = cost / Bomber.cost;

        var armyComp = new ArmyComp(0, 0, 0, 0, 0);

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
                                Console.WriteLine(armyComp);
                            }
                        }
                    }
                }
            }
        }

        return combinations;
    }
}

public class ArmyComp(
    int infantryCount,
    int artilleryCount,
    int tankCount,
    int fighterCount,
    int bomberCount
)
{
    public int InfantryCount { get; set; } = infantryCount;
    public int ArtilleryCount { get; set; } = artilleryCount;
    public int TankCount { get; set; } = tankCount;
    public int FighterCount { get; set; } = fighterCount;
    public int BomberCount { get; set; } = bomberCount;
    public int Cost =>
        InfantryCount * Infantry.cost
        + ArtilleryCount * Artillery.cost
        + TankCount * Tank.cost
        + FighterCount * Fighter.cost
        + BomberCount * Bomber.cost;

    public override string ToString()
    {
        return $"[{InfantryCount},{ArtilleryCount},{TankCount},{FighterCount},{BomberCount}] Cost: {Cost}";
    }

    public ArmyComp Clone()
    {
        return new ArmyComp(InfantryCount, ArtilleryCount, TankCount, FighterCount, BomberCount);
    }
}
