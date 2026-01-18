namespace axis_console_project.UnitTypes.Air;

public class Fighter : Unit
{
    public static int cost = 10;

    public Fighter(bool isAttacking)
    {
        this.Name = "Fighter";
        this.Cost = cost;
        this.Attack = 3;
        this.Defence = 4;
        this.isAttacking = isAttacking;
    }
}
