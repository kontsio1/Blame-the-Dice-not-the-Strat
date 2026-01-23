using axis_console_project.UnitTypes.Land;

namespace axis_console_project.BaseClasses;

public class Battle(Army attackingArmy, Army defendingArmy)
{
    // public Army attackingArmy = attackingArmy;
    // public Army defendingArmy = defendingArmy;

    public BattleInfo Fight()
    {
        var battleResult = BattleResult.Undecided;

        if (attackingArmy.GetType() == typeof(LandArmy))
        {
            var attackingNavalUnits = attackingArmy.GetAllBombardingNavalUnits();
            var bombardmentCasualties = 0;
            foreach (var navalUnit in attackingNavalUnits)
            {
                if (navalUnit.CanBombardLandUnits)
                {
                    var hit = navalUnit.Fire();
                    if (hit)
                    {
                        bombardmentCasualties++;
                    }
                }
            }
            defendingArmy.TakeCasualties(bombardmentCasualties);
        }

        int round = 1;

        while (battleResult == BattleResult.Undecided)
        {
            // Console.WriteLine($"\n--- Round {round} ---\n");

            // Console.WriteLine($"Attacking Army:");
            var casualtiesD = attackingArmy.Fire();
            // Console.WriteLine($"Attacking Army inflicted {casualtiesD} casualties.");

            // Console.WriteLine($"\nDefending Army:");
            var casualtiesA = defendingArmy.Fire();
            // Console.WriteLine($"Defending Army inflicted {casualtiesA} casualties.");

            defendingArmy.TakeCasualties(casualtiesD);
            attackingArmy.TakeCasualties(casualtiesA);

            battleResult = CheckWinConditions();
            round++;
        }

        var battleInfo = new BattleInfo(
            attackingArmy,
            defendingArmy,
            battleResult,
            attackingArmy.GetAllAliveUnits(),
            defendingArmy.GetAllAliveUnits()
        );
        return battleInfo;
    }

    public BattleResult CheckWinConditions()
    {
        if (!attackingArmy.HasUnitsAlive() && !defendingArmy.HasUnitsAlive())
        {
            // Console.WriteLine("\nBoth Armies have been defeated!");
            return BattleResult.Draw;
        }
        else if (!attackingArmy.HasUnitsAlive())
        {
            // Console.WriteLine("\nDefending Army wins!");
            return BattleResult.DefenderVictory;
        }
        else if (!defendingArmy.HasUnitsAlive())
        {
            // Console.WriteLine("\nAttacking Army wins!");
            return BattleResult.AttackerVictory;
        }
        return BattleResult.Undecided;
    }
}
