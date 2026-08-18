namespace axis_console_project.UnitTypes.Air;

public class Bomber : AirUnit
{
    public const int UnitCost = 12;

    public Bomber(bool isAttacking)
    {
        this.Name = "Bomber";
        Cost = UnitCost;
        this.Attack = 4;
        this.Defence = 1;
        this.isAttacking = isAttacking;
    }
}
