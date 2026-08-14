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
    : Army.Army(
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
            this.IsAttacking,
            this.Units.InfantryUnits.Count,
            this.Units.ArtilleryUnits.Count,
            this.Units.TankUnits.Count,
            this.Units.FighterUnits.Count,
            this.Units.BomberUnits.Count,
            this.Units.AntiAirUnits.Count,
            this.Units.CruiserUnits.Count,
            this.Units.BattleshipUnits.Count
        );
    }
}
