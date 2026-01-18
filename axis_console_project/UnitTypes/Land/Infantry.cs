namespace axis_console_project.UnitTypes.Land;

public class Infantry : Unit
{
    public static int cost = 3;

    public Infantry(bool isAttacking)
    {
        this.Name = $"Infantry";
        this.Cost = cost;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }

    public bool AccompaniedByArtillery { get; set; } = false;
    protected override int Attack
    {
        get { return AccompaniedByArtillery ? 2 : 1; }
    }
}
