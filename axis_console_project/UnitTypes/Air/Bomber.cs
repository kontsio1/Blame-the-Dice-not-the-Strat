namespace axis_console_project.UnitTypes.Air;

public class Bomber : AirUnit
{
    public static int cost = 12;

    public Bomber(bool isAttacking)
    {
        this.Name = "Bomber";
        this.Cost = cost;
        this.Attack = 4;
        this.Defence = 1;
        this.isAttacking = isAttacking;
    }
}
