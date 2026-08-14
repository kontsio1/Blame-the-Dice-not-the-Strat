namespace axis_console_project.Simulation;

public class UnitsStats(
    double infantryUnits = 0,
    double artilleryUnits = 0,
    double tankUnits = 0,
    double antiAirUnits = 0,
    double fighterUnits = 0,
    double bomberUnits = 0,
    double transportUnits = 0,
    double submarineUnits = 0,
    double destroyerUnits = 0,
    double cruiserUnits = 0,
    double battleshipUnits = 0,
    double aircraftCarrierUnits = 0
)
{
    public double InfantryUnits = infantryUnits;
    public double ArtilleryUnits = artilleryUnits;
    public double TankUnits = tankUnits;
    public double AntiAirUnits = antiAirUnits;
    public double FighterUnits = fighterUnits;
    public double BomberUnits = bomberUnits;
    public double TransportUnits = transportUnits;
    public double SubmarineUnits = submarineUnits;
    public double DestroyerUnits = destroyerUnits;
    public double CruiserUnits = cruiserUnits;
    public double BattleshipUnits = battleshipUnits;
    public double AircraftCarrierUnits = aircraftCarrierUnits;

    public override string ToString()
    {
        var parts = new List<string>();

        AddIfNotZero(parts, "Infantry", InfantryUnits);
        AddIfNotZero(parts, "Artillery", ArtilleryUnits);
        AddIfNotZero(parts, "Tanks", TankUnits);
        AddIfNotZero(parts, "AntiAir", AntiAirUnits);
        AddIfNotZero(parts, "Fighters", FighterUnits);
        AddIfNotZero(parts, "Bombers", BomberUnits);
        AddIfNotZero(parts, "Transports", TransportUnits);
        AddIfNotZero(parts, "Submarines", SubmarineUnits);
        AddIfNotZero(parts, "Destroyers", DestroyerUnits);
        AddIfNotZero(parts, "Cruisers", CruiserUnits);
        AddIfNotZero(parts, "Battleships", BattleshipUnits);
        AddIfNotZero(parts, "AircraftCarriers", AircraftCarrierUnits);

        return string.Join(",\n ", parts);
    }

    private void AddIfNotZero(List<string> parts, string name, double value)
    {
        if (value != 0)
            parts.Add($"{name}: {value:F2}");
    }
}
