using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.BaseClasses;

public class Army
{
    public Units units;
    public int Cost => units.Cost;
    public bool isAttacking;

    public Army(
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
    {
        this.units = new Units(
            isAttacking,
            infantryCount,
            artilleryCount,
            tankCount,
            fighterCount,
            bomberCount,
            antiAirCount,
            CruiserCount,
            BattleshipCount
        );
        this.isAttacking = isAttacking;
    }

    public List<Unit> GetAllUnits()
    {
        List<Unit> allUnits =
        [
            .. this.units.InfantryUnits,
            .. this.units.ArtilleryUnits,
            .. this.units.TankUnits,
            .. this.units.FighterUnits,
            .. this.units.BomberUnits,
            .. this.units.AntiAirUnits,
            .. this.units.CruiserUnits,
            .. this.units.BattleshipUnits,
        ];
        return allUnits;
    }

    public List<Unit> GetAllAliveUnits()
    {
        return GetAllUnits().Where(unit => unit.isAlive && unit.ParticipatesInBattle).ToList();
    }

    public bool HasUnitsAlive()
    {
        return GetAllUnits().Any(unit => unit.isAlive && unit.ParticipatesInBattle);
    }

    public List<NavalUnit> GetAllNavalUnits()
    {
        List<NavalUnit> navalUnits = [.. this.units.CruiserUnits, .. this.units.BattleshipUnits];
        return navalUnits;
    }

    public int Fire()
    {
        var casualties = 0;

        foreach (var unit in GetAllAliveUnits())
        {
            var hit = unit.Fire();
            if (hit)
            {
                casualties++;
            }
        }
        return casualties;
    }

    public void TakeCasualties(int casualties)
    {
        GetAllAliveUnits().Take(casualties).ToList().ForEach(unit => unit.TakeHit());
    }

    public static Units UnitsFromList(List<Unit> unitList)
    {
        var units = new Units(false, 0, 0, 0, 0, 0, 0, 0, 0);

        foreach (var unit in unitList)
        {
            switch (unit)
            {
                case Infantry infantry:
                    units.InfantryUnits.Add(infantry);
                    break;
                case Artillery artillery:
                    units.ArtilleryUnits.Add(artillery);
                    break;
                case Tank tank:
                    units.TankUnits.Add(tank);
                    break;
                case Fighter fighter:
                    units.FighterUnits.Add(fighter);
                    break;
                case Bomber bomber:
                    units.BomberUnits.Add(bomber);
                    break;
                case AntiAir antiAir:
                    units.AntiAirUnits.Add(antiAir);
                    break;
                case Cruiser cruiser:
                    units.CruiserUnits.Add(cruiser);
                    break;
                case Battleship battleship:
                    units.BattleshipUnits.Add(battleship);
                    break;
            }
        }
        return units;
    }

    public Army Clone()
    {
        return new Army(
            this.isAttacking,
            units.InfantryUnits.Count,
            units.ArtilleryUnits.Count,
            units.TankUnits.Count,
            units.FighterUnits.Count,
            units.BomberUnits.Count,
            units.AntiAirUnits.Count,
            units.CruiserUnits.Count,
            units.BattleshipUnits.Count
        );
    }
}
