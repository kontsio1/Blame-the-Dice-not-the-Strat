using axis_console_project.Armies;

namespace axis_console_project.UnitTypes.Land;

public class LandUnits(
    bool isAttacking,
    int infantryCount = 0,
    int artilleryCount = 0,
    int tankCount = 0
)
    : Units(
        isAttacking,
        infantryCount: infantryCount,
        artilleryCount: artilleryCount,
        tankCount: tankCount
    )
{
    public List<LandUnit> GetAllLandUnits()
    {
        List<LandUnit> allLandUnits = [.. InfantryUnits, .. ArtilleryUnits, .. TankUnits];
        return allLandUnits;
    }
}
