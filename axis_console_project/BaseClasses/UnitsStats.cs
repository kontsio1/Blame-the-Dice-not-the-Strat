public class UnitsStats(
    double infantryUnits,
    double artilleryUnits,
    double tankUnits,
    double fighterUnits,
    double bomberUnits
)
{
    public double InfantryUnits = infantryUnits;
    public double ArtilleryUnits = artilleryUnits;
    public double TankUnits = tankUnits;
    public double FighterUnits = fighterUnits;
    public double BomberUnits = bomberUnits;

    public override string ToString()
    {
        return $"Infantry: {InfantryUnits:F2},\n Artillery: {ArtilleryUnits:F2},\n Tanks: {TankUnits:F2},\n Fighters: {FighterUnits:F2},\n Bombers: {BomberUnits:F2}";
    }
}
