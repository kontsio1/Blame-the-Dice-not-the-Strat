namespace axis_console_project.UnitTypes.Sea;

public class Transport : NavalUnit
{
    public Transport(bool isAttacking)
    {
        this.Name = "Transport";
        this.Cost = 7;
        this.Attack = 0;
        this.Defence = 0;
        this.isAttacking = isAttacking;
    }
}
