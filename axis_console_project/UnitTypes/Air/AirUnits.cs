using axis_console_project.BaseClasses;

namespace axis_console_project.UnitTypes.Air;

public class AirUnits(
    bool isAttacking,
    int fighterCount = 0,
    int bomberCount = 0,
    int antiAirCount = 0
)
    : Units(
        isAttacking,
        fighterCount: fighterCount,
        bomberCount: bomberCount,
        antiAirCount: antiAirCount
    )
{
    public List<AirUnit> GetAllAirUnits()
    {
        List<AirUnit> allAirUnits = [.. FighterUnits, .. BomberUnits, .. AntiAirUnits];
        return allAirUnits;
    }
}
