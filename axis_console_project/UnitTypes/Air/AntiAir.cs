namespace axis_console_project.UnitTypes.Air;

public class AntiAir : Unit
{
    public static int cost = 6;

    public AntiAir(bool isAttacking)
    {
        this.Name = "Anti-Air";
        this.Cost = cost;
        this.Attack = 1;
        this.Defence = 0;
        this.isAttacking = isAttacking;
    }
}
