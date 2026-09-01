namespace axis_console_project.UnitTypes.Sea;

public class Cruiser : NavalUnit
{
    public const int UnitCost = 12;

    public Cruiser(bool isAttacking, bool participatesInBattle = true)
    {
        this.Name = "Cruiser";
        this.Cost = UnitCost;
        this.Attack = 3;
        this.Defence = 3;
        this.isAttacking = isAttacking;
        this.ParticipatesInBattle = participatesInBattle;
    }

    public override bool CanBombardLandUnits => true;
}
