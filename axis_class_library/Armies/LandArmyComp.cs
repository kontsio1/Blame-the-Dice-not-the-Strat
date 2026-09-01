// LandArmyComp.cs

using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;

namespace axis_console_project.Armies;

public class LandArmyComp(
    int infantryCount,
    int artilleryCount,
    int tankCount,
    int fighterCount,
    int bomberCount)
{
    public LandArmyComp(LandArmy army) : this(army.Units.InfantryUnits.Count, army.Units.ArtilleryUnits.Count, army.Units.TankUnits.Count, army.Units.FighterUnits.Count, army.Units.BomberUnits.Count)
    {
    }
    public int InfantryCount { get; set; } = infantryCount;
    public int ArtilleryCount { get; set; } = artilleryCount;
    public int TankCount { get; set; } = tankCount;
    public int FighterCount { get; set; } = fighterCount;
    public int BomberCount { get; set; } = bomberCount;

    public int Cost =>
        InfantryCount * Infantry.UnitCost
        + ArtilleryCount * Artillery.UnitCost
        + TankCount * Tank.UnitCost
        + FighterCount * Fighter.UnitCost
        + BomberCount * Bomber.UnitCost;

    public override string ToString()
    {
        return $"[I:{InfantryCount},A:{ArtilleryCount},T:{TankCount},F:{FighterCount},B:{BomberCount}] Cost: {Cost}";
    }

    public LandArmyComp Clone()
    {
        return new LandArmyComp(InfantryCount, ArtilleryCount, TankCount, FighterCount, BomberCount);
    }
}