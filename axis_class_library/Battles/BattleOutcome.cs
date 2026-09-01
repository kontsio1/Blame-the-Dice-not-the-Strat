using axis_console_project.Armies;

namespace axis_console_project.Battles;

using static Armies.Army;

public class BattleResult(
    Armies.Army attackingArmy,
    Armies.Army defendingArmy,
    BattleOutcome battleOutcome,
    List<Unit> attackerRemainingUnits,
    List<Unit> defenderRemainingUnits)
{
    public readonly Armies.Army AttackingArmy = attackingArmy;
    public readonly Armies.Army DefendingArmy = defendingArmy;
    public BattleOutcome BattleOutcome = battleOutcome;
    public Units AttackerRemainingUnits { get; set; } = UnitsFromList(attackerRemainingUnits);
    public Units DefenderRemainingUnits { get; set; } = UnitsFromList(defenderRemainingUnits);
}

public enum BattleOutcome
{
    AttackerVictory,
    DefenderVictory,
    Draw,
}
