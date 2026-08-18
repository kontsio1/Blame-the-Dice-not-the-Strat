namespace axis_console_project.UnitTypes.Sea;

public class Submarine : NavalUnit
{
    public const int UnitCost = 6;

    public Submarine(bool isAttacking)
    {
        this.Name = "Submarine";
        this.Cost = UnitCost;
        this.Attack = 2;
        this.Defence = 1;
        this.isAttacking = isAttacking;
    }
}
