// LandArmyBuilder.cs

using axis_console_project.Armies;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;

namespace axis_console_project.Resolvers;

public class LandArmyBuilder : ArmyBuilder
{
    public override IEnumerable<Army> CreateArmiesFromCost(int maxCost, bool isAttacking = true)
    {
        var armyComps = GetAllLandCombinations(maxCost);
        return armyComps.Select(c => new LandArmy(
            isAttacking,
            c.InfantryCount,
            c.ArtilleryCount,
            c.TankCount,
            c.FighterCount,
            c.BomberCount
        ));
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
                            var cheapestUnit = new LandArmy(true,1,1,1,1,1).GetAllUnits().MinBy(u => u.Cost);
                            if (armyComp.Cost <= cost && armyComp.Cost >= cost-cheapestUnit!.Cost && armyComp.Cost != 0)
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