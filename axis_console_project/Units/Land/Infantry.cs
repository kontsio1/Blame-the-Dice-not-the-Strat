public class Infantry : Unit
{
    public Infantry(bool isAttacking)
    {
        this.Name = $"Infantry";
        this.Cost = 3;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }

    public bool AccompaniedByArtillery { get; set; } = false;
    protected override int Attack
    {
        get { return AccompaniedByArtillery ? 2 : 1; }
    }
}
