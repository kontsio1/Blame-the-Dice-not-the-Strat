namespace axis_console_project.UnitTypes.Air;

public class Fighter : AirUnit
{
    public const int UnitCost = 10;

    public Fighter(bool isAttacking)
    {
        this.Name = "Fighter";
        Cost = UnitCost;
        this.Attack = 3;
        this.Defence = 4;
        this.isAttacking = isAttacking;
    }
}
