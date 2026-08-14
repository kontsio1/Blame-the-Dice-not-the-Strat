namespace axis_console_project.UnitTypes.Sea;

public class NavalArmada(
    bool isAttacking = false,
    int transportCount = 0,
    int submarineCount = 0,
    int destroyerCount = 0,
    int cruiserCount = 0,
    int battleshipCount = 0,
    int carrierCount = 0,
    int fighterCount = 0,
    int bomberCount = 0
)
    : Army.Army(
        isAttacking,
        transportCount: transportCount,
        submarineCount: submarineCount,
        destroyerCount: destroyerCount,
        cruiserCount: cruiserCount,
        battleshipCount: battleshipCount,
        aircraftCarrierCount: carrierCount,
        fighterCount: fighterCount,
        bomberCount: bomberCount
    )
{
    public override NavalArmada Clone()
    {
        return new NavalArmada(
            this.IsAttacking,
            this.Units.TransportUnits.Count,
            this.Units.SubmarineUnits.Count,
            this.Units.DestroyerUnits.Count,
            this.Units.CruiserUnits.Count,
            this.Units.BattleshipUnits.Count,
            this.Units.AircraftCarrierUnits.Count,
            this.Units.BomberUnits.Count,
            this.Units.FighterUnits.Count
        );
    }

    public bool PreventsSubSupriseAttack =>
        Units.SubmarineUnits.Count != 0 || Units.DestroyerUnits.Count != 0;

    public override void TakeCasualties(int casualties)
    {
        GetAllAliveUnits()
            .OrderByDescending(u => u.Health)
            .Take(casualties)
            .ToList()
            .ForEach(unit => unit.TakeHit());
    }
}
