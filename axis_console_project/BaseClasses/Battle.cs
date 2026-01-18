namespace axis_console_project.BaseClasses;

public class Battle(Army attackingArmy, Army defendingArmy)
{
    public Army AttackingArmy = attackingArmy;
    public Army DefendingArmy = defendingArmy;

    public BattleInfo Fight()
    {
        var battleResult = BattleResult.Undecided;

        var attackingNavalUnits = AttackingArmy
            .GetAllNavalUnits()
            .Where(unit => unit.ParticipatesInBattle)
            .ToList();
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

        int round = 1;

        while (battleResult == BattleResult.Undecided)
        {
            // Console.WriteLine($"\n--- Round {round} ---\n");

            // Console.WriteLine($"Attacking Army:");
            var casualtiesD = AttackingArmy.Fire();
            // Console.WriteLine($"Attacking Army inflicted {casualtiesD} casualties.");

            // Console.WriteLine($"\nDefending Army:");
            var casualtiesA = DefendingArmy.Fire();
            // Console.WriteLine($"Defending Army inflicted {casualtiesA} casualties.");

            DefendingArmy.TakeCasualties(casualtiesD);
            AttackingArmy.TakeCasualties(casualtiesA);

            battleResult = CheckWinConditions();
            round++;
        }

        var battleInfo = new BattleInfo(
            AttackingArmy,
            DefendingArmy,
            battleResult,
            AttackingArmy.GetAllAliveUnits(),
            DefendingArmy.GetAllAliveUnits()
        );
        return battleInfo;
    }

    public BattleResult CheckWinConditions()
    {
        if (!AttackingArmy.HasUnitsAlive() && !DefendingArmy.HasUnitsAlive())
        {
            Console.WriteLine("\nBoth Armies have been defeated!");
            return BattleResult.Draw;
        }
        else if (!AttackingArmy.HasUnitsAlive())
        {
            Console.WriteLine("\nDefending Army wins!");
            return BattleResult.DefenderVictory;
        }
        else if (!DefendingArmy.HasUnitsAlive())
        {
            Console.WriteLine("\nAttacking Army wins!");
            return BattleResult.AttackerVictory;
        }
        return BattleResult.Undecided;
    }
}
