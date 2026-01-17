public class Destroyer : NavalUnit
{
    public Destroyer(bool isAttacking)
    {
        this.Name = "Destroyer";
        this.Cost = 8;
        this.Attack = 2;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }
}
