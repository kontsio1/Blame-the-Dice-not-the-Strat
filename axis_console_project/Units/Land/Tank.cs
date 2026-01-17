public class Tank : Unit
{
    public Tank(bool isAttacking)
    {
        this.Name = "Tank";
        this.Cost = 5;
        this.Attack = 3;
        this.Defence = 3;
        this.isAttacking = isAttacking;
    }
}
