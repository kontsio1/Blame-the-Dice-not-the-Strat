// NavalArmadaBuilder.cs

using axis_console_project.Armies;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.Resolvers;

public class NavalArmadaBuilder : ArmyBuilder
{
    public override IEnumerable<Army> CreateArmiesFromCost(int maxCost, bool isAttacking = true)
    {
        var armyComps = GetAllNavalCombinations(maxCost);
        return armyComps.Select(c => new NavalArmada(
            isAttacking,
            c.TransportCount,
            c.SubmarineCount,
            c.DestroyerCount,
            c.CruiserCount,
            c.BattleshipCount,
            c.AircraftCarrierCount,
            c.FighterCount,
            c.BomberCount
        ));
    }

    private List<NavalArmadaComp> GetAllNavalCombinations(int cost)
    {
        var combinations = new List<NavalArmadaComp>();

        int maxTransport = cost / Transport.UnitCost;
        int maxSubmarine = cost / Submarine.UnitCost;
        int maxDestroyer = cost / Destroyer.UnitCost;
        int maxCruiser = cost / Cruiser.UnitCost;
        int maxBattleship = cost / Battleship.UnitCost;
        int maxAircraftCarrier = cost / AircraftCarrier.UnitCost;
        int maxFighter = cost / Fighter.UnitCost;
        int maxBomber = cost / Bomber.UnitCost;

        for (int transport = 0; transport <= maxTransport; transport++)
        {
            for (int submarine = 0; submarine <= maxSubmarine; submarine++)
            {
                for (int destroyer = 0; destroyer <= maxDestroyer; destroyer++)
                {
                    for (int cruiser = 0; cruiser <= maxCruiser; cruiser++)
                    {
                        for (int battleship = 0; battleship <= maxBattleship; battleship++)
                        {
                            for (int carrier = 0; carrier <= maxAircraftCarrier; carrier++)
                            {
                                for (int fighter = 0; fighter <= maxFighter; fighter++)
                                {
                                    for (int bomber = 0; bomber <= maxBomber; bomber++)
                                    {
                                        var armyComp = new NavalArmadaComp(
                                            transport, submarine, destroyer, cruiser, battleship, carrier, fighter,
                                            bomber
                                        );
                                        var cheapestUnit = new NavalArmada(true,1,1,1,1,1,1,1,1).GetAllUnits().MinBy(u => u.Cost);
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
                }
            }
        }
        return combinations;
    }
}