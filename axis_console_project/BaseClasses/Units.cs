using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_console_project.BaseClasses;

public class Units
{
    public List<AntiAir> AntiAirUnits;
    public List<Infantry> InfantryUnits;
    public List<Artillery> ArtilleryUnits;
    public List<Tank> TankUnits;
    public List<Fighter> FighterUnits;
    public List<Bomber> BomberUnits;
    public List<Cruiser> CruiserUnits;
    public List<Battleship> BattleshipUnits;
    public int Cost => GetTotalUnitCost();
    public bool IsAttacking;

    public Units(
        bool isAttacking,
        int infantryCount,
        int artilleryCount,
        int tankCount,
        int fighterCount,
        int bomberCount,
        int antiAirCount,
        int CruiserCount,
        int BattleshipCount
    )
    {
        InfantryUnits = new List<Infantry>();
        ArtilleryUnits = new List<Artillery>();
        TankUnits = new List<Tank>();
        FighterUnits = new List<Fighter>();
        BomberUnits = new List<Bomber>();
        AntiAirUnits = new List<AntiAir>();
        CruiserUnits = new List<Cruiser>();
        BattleshipUnits = new List<Battleship>();
        IsAttacking = isAttacking;

        for (int i = 0; i < antiAirCount; i++)
        {
            AntiAirUnits.Add(new AntiAir(isAttacking));
        }
        for (int i = 0; i < CruiserCount; i++)
        {
            CruiserUnits.Add(new Cruiser(isAttacking, false));
        }
        for (int i = 0; i < BattleshipCount; i++)
        {
            BattleshipUnits.Add(new Battleship(isAttacking, false));
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
    }

    private int GetTotalUnitCost()
    {
        return GetAllUnits().Where(u => u.ParticipatesInBattle).Sum(unit => unit.Cost);
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
            .. CruiserUnits,
            .. BattleshipUnits,
        ];
        return allUnits;
    }

    public override string ToString()
    {
        return $"Infantry:{InfantryUnits.Count}, Artillery:{ArtilleryUnits.Count}, Tanks:{TankUnits.Count}, Fighters:{FighterUnits.Count}, Bombers:{BomberUnits.Count}, TotalCost:{Cost}";
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
            CruiserUnits.Count,
            BattleshipUnits.Count
        );
    }
}
