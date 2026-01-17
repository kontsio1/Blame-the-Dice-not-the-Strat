public class Battleship : NavalUnit
{
    public Battleship(bool isAttacking, bool participatesInBattle = true)
    {
        this.Name = "Battleship";
        this.Cost = 20;
        this.Attack = 4;
        this.Defence = 4;
        this.Health = 2;
        this.isAttacking = isAttacking;
        this.ParticipatesInBattle = participatesInBattle;
    }

    public override bool CanBombardLandUnits => true;
}
