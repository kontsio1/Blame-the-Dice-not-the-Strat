using axis_console_project.BaseClasses;

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
    : Army(
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
            this.isAttacking,
            this.units.TransportUnits.Count,
            this.units.SubmarineUnits.Count,
            this.units.DestroyerUnits.Count,
            this.units.CruiserUnits.Count,
            this.units.BattleshipUnits.Count,
            this.units.AircraftCarrierUnits.Count,
            this.units.BomberUnits.Count,
            this.units.FighterUnits.Count
        );
    }

    public bool PreventsSubSupriseAttack =>
        units.SubmarineUnits.Count != 0 || units.DestroyerUnits.Count != 0;

    public override void TakeCasualties(int casualties)
    {
        GetAllAliveUnits()
            .OrderByDescending(u => u.Health)
            .Take(casualties)
            .ToList()
            .ForEach(unit => unit.TakeHit());
    }
}
