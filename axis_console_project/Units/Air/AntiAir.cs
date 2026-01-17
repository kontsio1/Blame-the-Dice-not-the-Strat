public class AntiAir : Unit
{
    public AntiAir(bool isAttacking)
    {
        this.Name = "Anti-Air";
        this.Cost = 6;
        this.Attack = 1;
        this.Defence = 0;
        this.isAttacking = isAttacking;
    }
}
