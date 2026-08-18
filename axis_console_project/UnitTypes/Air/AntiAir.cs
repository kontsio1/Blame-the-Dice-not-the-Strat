namespace axis_console_project.UnitTypes.Air;

public class AntiAir : AirUnit
{
    public const int UnitCost = 6;
    private readonly Random _dice = new();

    public AntiAir(bool isAttacking)
    {
        this.Name = "Anti-Air";
        Cost = UnitCost;
        this.Attack = 1;
        this.Defence = 0;
        this.isAttacking = isAttacking;
    }

    public override bool Fire()
    {
        // Anti-Air units do not fire during normal attack phases
        return false;
    }

    public bool DefendAgainstAirAttack()
    {
        if (isAlive)
        {
            int roll = _dice.Next(1, 7);
            return roll <= Attack;
        }
        return false;
    }
}
