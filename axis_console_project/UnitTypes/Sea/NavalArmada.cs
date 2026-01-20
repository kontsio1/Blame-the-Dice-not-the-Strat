using axis_console_project.BaseClasses;

namespace axis_console_project.UnitTypes.Sea;

public class NavalArmada(
    bool isAttacking = false,
    int transportCount = 0,
    int submarineCount = 0,
    int destroyerCount = 0,
    int cruiserCount = 0,
    int battleshipCount = 0,
    int carrierCount = 0
)
    : Army(
        isAttacking,
        transportCount: transportCount,
        submarineCount: submarineCount,
        destroyerCount: destroyerCount,
        cruiserCount: cruiserCount,
        battleshipCount: battleshipCount,
        aircraftCarrierCount: carrierCount
    ) { }
