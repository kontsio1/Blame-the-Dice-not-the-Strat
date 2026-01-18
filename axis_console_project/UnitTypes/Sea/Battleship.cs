namespace axis_console_project.UnitTypes.Sea;

public class Battleship : NavalUnit
{
    public static int cost = 20;

    public Battleship(bool isAttacking, bool participatesInBattle = true)
    {
        this.Name = "Battleship";
        this.Cost = cost;
        this.Attack = 4;
        this.Defence = 4;
        this.Health = 2;
        this.isAttacking = isAttacking;
        this.ParticipatesInBattle = participatesInBattle;
    }

    public override bool CanBombardLandUnits => true;
}
