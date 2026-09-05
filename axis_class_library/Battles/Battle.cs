using axis_console_project.Armies;
using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.Battles;

public class Battle
{
    public readonly Army AttackingArmy;
    public readonly Army DefendingArmy;
    public BattleOutcome? Outcome;
    public Battle(Army attackingArmy, Army defendingArmy)
    {
        this.AttackingArmy = attackingArmy;
        this.DefendingArmy = defendingArmy;
    }
    public BattleResult Fight(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (AttackingArmy is LandArmy)
        {
            AntiAirDefense(cancellationToken);
            NavalBombardment(cancellationToken);
        }
        if (DefendingArmy is NavalArmada { PreventsSubSupriseAttack: false })
        {
            SubSurpriseAttack(cancellationToken);
        }
        while (Outcome is null)
        {
            cancellationToken.ThrowIfCancellationRequested();
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
        else if (!AttackingArmy.HasUnitsAlive())
        {
            Outcome = BattleOutcome.DefenderVictory;
        }
        else if (!DefendingArmy.HasUnitsAlive())
        {
            Outcome = BattleOutcome.AttackerVictory;
        }
    }

    private void NavalBombardment(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (AttackingArmy.GetType() == typeof(LandArmy))
        {
            var attackingNavalUnits = AttackingArmy.GetAllBombardingNavalUnits();
            var bombardmentCasualties = 0;
            foreach (var navalUnit in attackingNavalUnits)
            {
                cancellationToken.ThrowIfCancellationRequested();

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

    private void AntiAirDefense(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var antiAir = DefendingArmy.GetAllUnits().FirstOrDefault(e => e is AntiAir) as AntiAir;
        if (antiAir != null)
        {
            var air = AttackingArmy.GetAllAliveUnits().Where(unit => unit is AirUnit).ToList();

            foreach (var u in air)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var hit = antiAir.DefendAgainstAirAttack();
                if (hit)
                {
                    u.TakeHit();
                }
            }
        }
    }

    private void SubSurpriseAttack(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var subs = AttackingArmy.Units.SubmarineUnits.ToList();
        var casualties = 0;

        foreach (var sub in subs)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var hit = sub.Fire();
            if (hit)
            {
                casualties++;
            }
        }
        DefendingArmy.TakeCasualties(casualties);
    }
}
