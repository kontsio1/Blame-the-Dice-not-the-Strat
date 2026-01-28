using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.BaseClasses;

public abstract class Army
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
        int transportCount = 0,
        int submarineCount = 0,
        int destroyerCount = 0,
        int cruiserCount = 0,
        int battleshipCount = 0,
        int aircraftCarrierCount = 0
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
            transportCount,
            submarineCount,
            destroyerCount,
            cruiserCount,
            battleshipCount,
            aircraftCarrierCount
        );
        this.isAttacking = isAttacking;
    }

    public List<Unit> GetAllUnits()
    {
        List<Unit> allUnits = [];
        //change order for optimal attack and defence
        if (isAttacking)
        {
            allUnits =
            [
                .. this.units.AntiAirUnits, //0
                .. this.units.InfantryUnits, //1
                .. this.units.ArtilleryUnits, //2
                .. this.units.TankUnits, //3
                .. this.units.TransportUnits, //0
                .. this.units.AircraftCarrierUnits, //1
                .. this.units.SubmarineUnits, //2
                .. this.units.DestroyerUnits, //2
                .. this.units.FighterUnits, //3
                .. this.units.CruiserUnits, //3
                .. this.units.BomberUnits, //4
                .. this.units.BattleshipUnits, //4
            ];
        }
        else
        {
            allUnits =
            [
                .. this.units.AntiAirUnits, //0
                .. this.units.TransportUnits, //0
                .. this.units.SubmarineUnits, //1
                .. this.units.InfantryUnits, //2
                .. this.units.ArtilleryUnits, //2
                .. this.units.DestroyerUnits, //2
                .. this.units.AircraftCarrierUnits, //2
                .. this.units.BomberUnits, //2
                .. this.units.TankUnits, //3
                .. this.units.CruiserUnits, //3
                .. this.units.FighterUnits, //4
                .. this.units.BattleshipUnits, //4
            ];
        }
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

    public List<NavalUnit> GetAllBombardingNavalUnits()
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

    public virtual void TakeCasualties(int casualties)
    {
        GetAllAliveUnits().Take(casualties).ToList().ForEach(unit => unit.TakeHit());
    }

    public static Units UnitsFromList(List<Unit> unitList)
    {
        var units = new Units(false);

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
                case Transport transport:
                    units.TransportUnits.Add(transport);
                    break;
                case Submarine submarine:
                    units.SubmarineUnits.Add(submarine);
                    break;
                case Destroyer destroyer:
                    units.DestroyerUnits.Add(destroyer);
                    break;
                case Cruiser cruiser:
                    units.CruiserUnits.Add(cruiser);
                    break;
                case Battleship battleship:
                    units.BattleshipUnits.Add(battleship);
                    break;
                case AircraftCarrier aircraftCarrier:
                    units.AircraftCarrierUnits.Add(aircraftCarrier);
                    break;
            }
        }
        return units;
    }

    public abstract Army Clone();
}
