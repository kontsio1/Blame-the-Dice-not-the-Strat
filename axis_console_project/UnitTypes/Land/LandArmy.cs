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
    int CruiserCount = 0,
    int BattleshipCount = 0
)
    : Army(
        isAttacking,
        infantryCount,
        artilleryCount,
        tankCount,
        fighterCount,
        bomberCount,
        antiAirCount,
        CruiserCount,
        BattleshipCount
    ) { }
