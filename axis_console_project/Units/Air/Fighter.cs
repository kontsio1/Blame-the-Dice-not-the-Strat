public class Fighter : Unit
{
    public Fighter(bool isAttacking)
    {
        this.Name = "Fighter";
        this.Cost = 10;
        this.Attack = 3;
        this.Defence = 4;
        this.isAttacking = isAttacking;
    }
}
