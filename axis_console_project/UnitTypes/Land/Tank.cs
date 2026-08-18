namespace axis_console_project.UnitTypes.Land;

public class Tank : LandUnit
{
    public const int UnitCost = 5;

    public Tank(bool isAttacking)
    {
        this.Name = "Tank";
        this.Cost = UnitCost;
        this.Attack = 3;
        this.Defence = 3;
        this.isAttacking = isAttacking;
    }
}
