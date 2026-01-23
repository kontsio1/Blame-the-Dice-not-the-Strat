using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.BaseClasses;

public class Units
{
    public List<AntiAir> AntiAirUnits = new();
    public List<Infantry> InfantryUnits = new();
    public List<Artillery> ArtilleryUnits = new();
    public List<Tank> TankUnits = new();

    public List<Fighter> FighterUnits = new();
    public List<Bomber> BomberUnits = new();

    public List<Transport> TransportUnits = new();
    public List<Submarine> SubmarineUnits = new();
    public List<Destroyer> DestroyerUnits = new();
    public List<Cruiser> CruiserUnits = new();
    public List<Battleship> BattleshipUnits = new();
    public List<AircraftCarrier> AircraftCarrierUnits = new();
    public int Cost => GetTotalUnitCost();
    public bool IsAttacking;

    public Units(
        bool isAttacking = true,
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
        IsAttacking = isAttacking;

        for (int i = 0; i < antiAirCount; i++)
        {
            AntiAirUnits.Add(new AntiAir(isAttacking));
        }
        for (int i = 0; i < infantryCount; i++)
        {
            InfantryUnits.Add(new Infantry(isAttacking));
        }
        for (int i = 0; i < artilleryCount; i++)
        {
            ArtilleryUnits.Add(new Artillery(isAttacking));
            var unaccInfantryUnits = InfantryUnits
                .Where(infantry => !infantry.AccompaniedByArtillery)
                .ToList();
            if (unaccInfantryUnits.Count > 0)
            {
                unaccInfantryUnits[0].AccompaniedByArtillery = true;
            }
        }
        for (int i = 0; i < tankCount; i++)
        {
            TankUnits.Add(new Tank(isAttacking));
        }
        for (int i = 0; i < fighterCount; i++)
        {
            FighterUnits.Add(new Fighter(isAttacking));
        }
        for (int i = 0; i < bomberCount; i++)
        {
            BomberUnits.Add(new Bomber(isAttacking));
        }
        for (int i = 0; i < transportCount; i++)
        {
            TransportUnits.Add(new Transport(isAttacking));
        }
        for (int i = 0; i < submarineCount; i++)
        {
            SubmarineUnits.Add(new Submarine(isAttacking));
        }
        for (int i = 0; i < destroyerCount; i++)
        {
            DestroyerUnits.Add(new Destroyer(isAttacking));
        }
        for (int i = 0; i < cruiserCount; i++)
        {
            CruiserUnits.Add(new Cruiser(isAttacking, false));
        }
        for (int i = 0; i < battleshipCount; i++)
        {
            BattleshipUnits.Add(new Battleship(isAttacking, false));
        }
        for (int i = 0; i < aircraftCarrierCount; i++)
        {
            AircraftCarrierUnits.Add(new AircraftCarrier(isAttacking));
        }
    }

    private int GetTotalUnitCost()
    {
        return GetAllUnits().Sum(unit => unit.Cost);
    }

    public List<Unit> GetAllUnits()
    {
        List<Unit> allUnits =
        [
            .. InfantryUnits,
            .. ArtilleryUnits,
            .. TankUnits,
            .. FighterUnits,
            .. BomberUnits,
            .. AntiAirUnits,
            .. SubmarineUnits,
            .. DestroyerUnits,
            .. CruiserUnits,
            .. BattleshipUnits,
            .. AircraftCarrierUnits,
        ];
        return allUnits;
    }

    public override string ToString()
    {
        return $"Infantry:{InfantryUnits.Count}, Artillery:{ArtilleryUnits.Count}, Tanks:{TankUnits.Count}, AntiairUnits:{AntiAirUnits.Count}, Fighters:{FighterUnits.Count}, Bombers:{BomberUnits.Count}, Submarines:{SubmarineUnits.Count}, Destroyers:{DestroyerUnits.Count}, Cruisers:{CruiserUnits.Count}, Battleships:{BattleshipUnits.Count}, AircraftCarriers:{AircraftCarrierUnits.Count}, TotalCost:{Cost}";
    }

    public Units Clone()
    {
        return new Units(
            IsAttacking,
            InfantryUnits.Count,
            ArtilleryUnits.Count,
            TankUnits.Count,
            FighterUnits.Count,
            BomberUnits.Count,
            AntiAirUnits.Count,
            SubmarineUnits.Count,
            DestroyerUnits.Count,
            CruiserUnits.Count,
            BattleshipUnits.Count,
            AircraftCarrierUnits.Count
        );
    }
}
