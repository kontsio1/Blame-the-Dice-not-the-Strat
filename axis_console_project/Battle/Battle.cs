using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.Battle;

public class Battle
{
    public readonly Army.Army AttackingArmy;
    public readonly Army.Army DefendingArmy;
    public BattleOutcome? Outcome;
    public Battle(Army.Army attackingArmy, Army.Army defendingArmy)
    {
        this.AttackingArmy = attackingArmy;
        this.DefendingArmy = defendingArmy;
    }
    private int roundNo;
    public BattleResult Fight()
    {
        if (AttackingArmy is LandArmy)
        {
            AntiAirDefense();
            NavalBombardment();
        }
        if (DefendingArmy is NavalArmada { PreventsSubSupriseAttack: false })
        {
            SubSurpriseAttack();
        }
        while (Outcome is null)
        {
            ++roundNo;
            var casualtiesD = AttackingArmy.Fire();
            var casualtiesA = DefendingArmy.Fire();

            DefendingArmy.TakeCasualties(casualtiesD);
            AttackingArmy.TakeCasualties(casualtiesA);

            CheckWinConditions();
        }

        var battleInfo = new BattleResult(
            AttackingArmy,
            DefendingArmy,
            Outcome ?? BattleOutcome.Draw,
            AttackingArmy.GetAllAliveUnits(),
            DefendingArmy.GetAllAliveUnits()
        );
        return battleInfo;
    }

    private void CheckWinConditions()
    {
        if (!AttackingArmy.HasUnitsAlive() && !DefendingArmy.HasUnitsAlive())
        {
            Outcome = BattleOutcome.Draw;
        }
        if (!AttackingArmy.HasUnitsAlive())
        {
            Outcome = BattleOutcome.DefenderVictory;
        }
        if (!DefendingArmy.HasUnitsAlive())
        {
            Outcome = BattleOutcome.AttackerVictory;
        }
    }

    private void NavalBombardment()
    {
        if (AttackingArmy.GetType() == typeof(LandArmy))
        {
            var attackingNavalUnits = AttackingArmy.GetAllBombardingNavalUnits();
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
            DefendingArmy.TakeCasualties(bombardmentCasualties);
        }
    }

    private void AntiAirDefense()
    {
        var antiAir = DefendingArmy.GetAllUnits().FirstOrDefault(e => e is AntiAir) as AntiAir;
        if (antiAir != null)
        {
            var air = AttackingArmy.GetAllAliveUnits().Where(unit => unit is AirUnit).ToList();

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

    private void SubSurpriseAttack()
    {
        var subs = AttackingArmy.Units.SubmarineUnits.ToList();
        var casualties = 0;

        foreach (var sub in subs)
        {
            var hit = sub.Fire();
            if (hit)
            {
                casualties++;
            }
        }
        DefendingArmy.TakeCasualties(casualties);
    }
}
