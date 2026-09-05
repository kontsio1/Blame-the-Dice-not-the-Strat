using axis_console_project.Armies;

namespace axis_console_project.Battles;

using static Army;

public class BattleResult(
    BattleOutcome battleOutcome,
    List<Unit> attackerRemainingUnits,
    List<Unit> defenderRemainingUnits)
{
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
