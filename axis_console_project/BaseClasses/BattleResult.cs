using static Army;

public class BattleInfo
{
    public Army AttackingArmy { get; set; }
    public Army DefendingArmy { get; set; }
    public BattleResult Result { get; set; }
    public Units AttackerRemainingUnits { get; set; }
    public Units DefenderRemainingUnits { get; set; }

    public BattleInfo(
        Army attackingArmy,
        Army defendingArmy,
        BattleResult result,
        List<Unit> attackerRemainingUnits,
        List<Unit> defenderRemainingUnits
    )
    {
        AttackingArmy = attackingArmy;
        DefendingArmy = defendingArmy;
        Result = result;
        AttackerRemainingUnits = UnitsFromList(attackerRemainingUnits);
        DefenderRemainingUnits = UnitsFromList(defenderRemainingUnits);
    }
}

public enum BattleResult
{
    Undecided,
    AttackerVictory,
    DefenderVictory,
    Draw,
}
