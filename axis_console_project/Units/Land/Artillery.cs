public class Artillery : Unit
{
    public Artillery(bool isAttacking)
    {
        this.Name = "Artillery";
        this.Cost = 4;
        this.Attack = 2;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }
}
