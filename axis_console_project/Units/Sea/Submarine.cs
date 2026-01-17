public class Submarine : NavalUnit
{
    public Submarine(bool isAttacking)
    {
        this.Name = "Submarine";
        this.Cost = 6;
        this.Attack = 2;
        this.Defence = 1;
        this.isAttacking = isAttacking;
    }

    public override bool CanSurpriseAttack => true;
}
