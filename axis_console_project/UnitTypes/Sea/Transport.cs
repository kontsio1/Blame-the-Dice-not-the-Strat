namespace axis_console_project.UnitTypes.Sea;

public class Transport : NavalUnit
{
    public const int UnitCost = 7;

    public Transport(bool isAttacking)
    {
        this.Name = "Transport";
        Cost = UnitCost;
        this.Attack = 0;
        this.Defence = 0;
        this.isAttacking = isAttacking;
    }
}
