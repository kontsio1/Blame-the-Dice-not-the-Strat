using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;

namespace axis_console_project.BaseClasses;

public class Battle(Army attackingArmy, Army defendingArmy)
{
    // public Army attackingArmy = attackingArmy;
    // public Army defendingArmy = defendingArmy;

    public BattleInfo Fight()
    {
        var battleResult = BattleResult.Undecided;

        AntiAirDefense();
        NavalBombardment();

        int round = 1;

        while (battleResult == BattleResult.Undecided)
        {
            var casualtiesD = attackingArmy.Fire();
            var casualtiesA = defendingArmy.Fire();

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
            return BattleResult.Draw;
        }
        else if (!attackingArmy.HasUnitsAlive())
        {
            return BattleResult.DefenderVictory;
        }
        else if (!defendingArmy.HasUnitsAlive())
        {
            return BattleResult.AttackerVictory;
        }
        return BattleResult.Undecided;
    }

    private void NavalBombardment()
    {
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
    }

    private void AntiAirDefense()
    {
        var antiAir = defendingArmy.GetAllUnits().FirstOrDefault(e => e is AntiAir) as AntiAir;
        if (attackingArmy.GetType() == typeof(LandArmy) && antiAir != null)
        {
            var air = attackingArmy.GetAllAliveUnits().Where(unit => unit is AirUnit).ToList();

            foreach (var u in air)
            {
                var hit = antiAir.DefendAgainstAirAttack();
                if (hit)
                {
                    u.TakeHit();
                }
            }
        }
    }
}
