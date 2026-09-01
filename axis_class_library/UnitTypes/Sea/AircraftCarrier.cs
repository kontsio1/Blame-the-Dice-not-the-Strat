namespace axis_console_project.UnitTypes.Sea;

public class AircraftCarrier : NavalUnit
{
    public const int UnitCost = 14;

    public AircraftCarrier(bool isAttacking)
    {
        this.Name = "Aircraft Carrier";
        this.Cost = UnitCost;
        this.Attack = 1;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }

    public override int FighterCapacity => 2;
}
