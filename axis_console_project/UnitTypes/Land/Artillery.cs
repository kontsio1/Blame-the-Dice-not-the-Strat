namespace axis_console_project.UnitTypes.Land;

public class Artillery : LandUnit
{
    public static int cost = 4;

    public Artillery(bool isAttacking)
    {
        this.Name = "Artillery";
        this.Cost = cost;
        this.Attack = 2;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }
}
