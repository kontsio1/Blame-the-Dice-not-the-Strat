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
            return $"Infantry: {InfantryUnits.Count},\n Artillery: {ArtilleryUnits.Count},\n Tanks: {TankUnits.Count},\n Fighters: {FighterUnits.Count},\n Bombers: {BomberUnits.Count}";
        }
    }

    public static Units UnitsFromList(List<Unit> unitList)
    {
        var units = new Units(false, 0, 0, 0, 0, 0, 0, 0, 0);

        foreach (var unit in unitList)
        {
            switch (unit)
            {
                case Infantry infantry:
                    units.InfantryUnits.Add((Infantry)unit);
                    break;
                case Artillery artillery:
                    units.ArtilleryUnits.Add((Artillery)unit);
                    break;
                case Tank tank:
                    units.TankUnits.Add((Tank)unit);
                    break;
                case Fighter fighter:
                    units.FighterUnits.Add((Fighter)unit);
                    break;
                case Bomber bomber:
                    units.BomberUnits.Add((Bomber)unit);
                    break;
                case AntiAir antiAir:
                    units.AntiAirUnits.Add((AntiAir)unit);
                    break;
                case Cruiser cruiser:
                    units.CruiserUnits.Add((Cruiser)unit);
                    break;
                case Battleship battleship:
                    units.BattleshipUnits.Add((Battleship)unit);
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
