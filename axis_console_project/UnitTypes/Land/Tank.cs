namespace axis_console_project.UnitTypes.Land;

public class Tank : LandUnit
{
    public static int cost = 5;

    public Tank(bool isAttacking)
    {
        this.Name = "Tank";
        this.Cost = cost;
        this.Attack = 3;
        this.Defence = 3;
        this.isAttacking = isAttacking;
    }
}
