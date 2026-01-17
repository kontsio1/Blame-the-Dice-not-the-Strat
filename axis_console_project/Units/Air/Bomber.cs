public class Bomber : Unit
{
    public Bomber(bool isAttacking)
    {
        this.Name = "Bomber";
        this.Cost = 12;
        this.Attack = 4;
        this.Defence = 1;
        this.isAttacking = isAttacking;
    }
}
