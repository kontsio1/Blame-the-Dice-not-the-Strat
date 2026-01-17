public class AircraftCarrier : NavalUnit
{
    public AircraftCarrier(bool isAttacking)
    {
        this.Name = "Aircraft Carrier";
        this.Cost = 14;
        this.Attack = 1;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }

    public override int FighterCapacity => 2;
}
