namespace axis_console_project.UnitTypes.Sea;

public class Destroyer : NavalUnit
{
    public const int UnitCost = 8;

    public Destroyer(bool isAttacking)
    {
        this.Name = "Destroyer";
        this.Cost = UnitCost;
        this.Attack = 2;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }
}
