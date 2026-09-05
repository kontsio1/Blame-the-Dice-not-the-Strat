namespace axis_console_project.UnitTypes.Land;

public sealed class Artillery : LandUnit
{
    public const int UnitCost = 4;

    public Artillery(bool isAttacking)
    {
        this.Name = "Artillery";
        Cost = UnitCost;
        this.Attack = 2;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }
}
