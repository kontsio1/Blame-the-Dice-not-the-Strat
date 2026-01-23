using axis_console_project.BaseClasses;

namespace axis_console_project.UnitTypes.Land;

public class LandArmy(
    bool isAttacking = false,
    int infantryCount = 0,
    int artilleryCount = 0,
    int tankCount = 0,
    int fighterCount = 0,
    int bomberCount = 0,
    int antiAirCount = 0,
    int cruiserCount = 0,
    int battleshipCount = 0
)
    : Army(
        isAttacking,
        infantryCount,
        artilleryCount,
        tankCount,
        fighterCount,
        bomberCount,
        antiAirCount,
        cruiserCount: cruiserCount,
        battleshipCount: battleshipCount
    )
{
    public override LandArmy Clone()
    {
        return new LandArmy(
            this.isAttacking,
            this.units.InfantryUnits.Count,
            this.units.ArtilleryUnits.Count,
            this.units.TankUnits.Count,
            this.units.FighterUnits.Count,
            this.units.BomberUnits.Count,
            this.units.AntiAirUnits.Count,
            this.units.CruiserUnits.Count,
            this.units.BattleshipUnits.Count
        );
    }
}
