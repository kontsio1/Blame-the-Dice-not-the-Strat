// NavalArmadaComp.cs

using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.Armies;

public class NavalArmadaComp(
    int transportCount,
    int submarineCount,
    int destroyerCount,
    int cruiserCount,
    int battleshipCount,
    int carrierCount,
    int fighterCount,
    int bomberCount)
{
    public NavalArmadaComp(NavalArmada armada) : this(armada.Units.TransportUnits.Count, armada.Units.SubmarineUnits.Count, armada.Units.DestroyerUnits.Count, armada.Units.CruiserUnits.Count, armada.Units.BattleshipUnits.Count, armada.Units.AircraftCarrierUnits.Count, armada.Units.FighterUnits.Count, armada.Units.BomberUnits.Count)
    {
    }
    public int TransportCount { get; set; } = transportCount;
    public int SubmarineCount { get; set; } = submarineCount;
    public int DestroyerCount { get; set; } = destroyerCount;
    public int CruiserCount { get; set; } = cruiserCount;
    public int BattleshipCount { get; set; } = battleshipCount;
    public int AircraftCarrierCount { get; set; } = carrierCount;
    public int FighterCount { get; set; } = fighterCount;
    public int BomberCount { get; set; } = bomberCount;

    public int Cost =>
        TransportCount * Transport.UnitCost
        + SubmarineCount * Submarine.UnitCost
        + DestroyerCount * Destroyer.UnitCost
        + CruiserCount * Cruiser.UnitCost
        + BattleshipCount * Battleship.UnitCost
        + AircraftCarrierCount * AircraftCarrier.UnitCost
        + FighterCount * Fighter.UnitCost
        + BomberCount * Bomber.UnitCost;

    public override string ToString()
    {
        return $"[T:{TransportCount},S:{SubmarineCount},D:{DestroyerCount},C:{CruiserCount},B:{BattleshipCount},C:{AircraftCarrierCount},F:{FighterCount},B:{BomberCount}] Cost: {Cost}";
    }

    public NavalArmadaComp Clone()
    {
        return new NavalArmadaComp(TransportCount, SubmarineCount, DestroyerCount, CruiserCount, BattleshipCount, AircraftCarrierCount, FighterCount, BomberCount);
    }
}