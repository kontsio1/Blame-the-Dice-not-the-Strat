public class Cruiser : NavalUnit
{
    public Cruiser(bool isAttacking, bool participatesInBattle = true)
    {
        this.Name = "Cruiser";
        this.Cost = 12;
        this.Attack = 3;
        this.Defence = 3;
        this.isAttacking = isAttacking;
        this.ParticipatesInBattle = participatesInBattle;
    }

    public override bool CanBombardLandUnits => true;
}
