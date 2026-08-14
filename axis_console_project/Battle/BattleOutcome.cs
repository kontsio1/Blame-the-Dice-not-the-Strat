using axis_console_project.Army;
using axis_console_project.UnitTypes;

namespace axis_console_project.Battle;

using static Army.Army;

public class BattleResult(
    Army.Army attackingArmy,
    Army.Army defendingArmy,
    BattleOutcome battleOutcome,
    List<Unit> attackerRemainingUnits,
    List<Unit> defenderRemainingUnits)
{
    public readonly Army.Army AttackingArmy = attackingArmy;
    public readonly Army.Army DefendingArmy = defendingArmy;
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
