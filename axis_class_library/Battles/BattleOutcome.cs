using axis_console_project.Armies;

namespace axis_console_project.Battles;

using static Army;

public class BattleResult(
    Army attackingArmy,
    Army defendingArmy,
    BattleOutcome battleOutcome,
    List<Unit> attackerRemainingUnits,
    List<Unit> defenderRemainingUnits)
{
    public readonly Army AttackingArmy = attackingArmy;
    public readonly Army DefendingArmy = defendingArmy;
    public readonly BattleOutcome BattleOutcome = battleOutcome;
    public Units AttackerRemainingUnits { get; set; } = UnitsFromList(attackerRemainingUnits);
    public Units DefenderRemainingUnits { get; set; } = UnitsFromList(defenderRemainingUnits);
}

public enum BattleOutcome
{
    AttackerVictory,
    DefenderVictory,
    Draw,
}
