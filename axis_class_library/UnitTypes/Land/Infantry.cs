namespace axis_console_project.UnitTypes.Land;

public sealed class Infantry : LandUnit
{
    public const int UnitCost = 3;

    public Infantry(bool isAttacking)
    {
        this.Name = $"Infantry";
        this.Cost = UnitCost;
        this.Defence = 2;
        this.isAttacking = isAttacking;
    }

    public bool AccompaniedByArtillery { get; set; } = false;
    protected override int Attack => AccompaniedByArtillery ? 2 : 1;
}
