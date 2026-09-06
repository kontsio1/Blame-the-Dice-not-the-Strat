using axis_console_project.Armies;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;

namespace axis_mobile_app.PageModels;

public static class ActualOutcomeAnalysisHelper
{
    public static void AdjustUnitCount(OutcomeUnitsInput input, string key, int delta)
    {
        switch (key)
        {
            case "infantry":
                input.Infantry = Clamp(input.Infantry + delta);
                break;
            case "artillery":
                input.Artillery = Clamp(input.Artillery + delta);
                break;
            case "tank":
                input.Tank = Clamp(input.Tank + delta);
                break;
            case "fighter":
                input.Fighter = Clamp(input.Fighter + delta);
                break;
            case "bomber":
                input.Bomber = Clamp(input.Bomber + delta);
                break;
            case "antiair":
                input.AntiAir = Clamp(input.AntiAir + delta);
                break;
            case "transport":
                input.Transport = Clamp(input.Transport + delta);
                break;
            case "submarine":
                input.Submarine = Clamp(input.Submarine + delta);
                break;
            case "destroyer":
                input.Destroyer = Clamp(input.Destroyer + delta);
                break;
            case "cruiser":
                input.Cruiser = Clamp(input.Cruiser + delta);
                break;
            case "battleship":
                input.Battleship = Clamp(input.Battleship + delta);
                break;
            case "carrier":
                input.Carrier = Clamp(input.Carrier + delta);
                break;
        }
    }

    public static LandArmy CreateLandArmy(OutcomeUnitsInput input, bool isAttacking)
    {
        return new LandArmy(
            isAttacking: isAttacking,
            infantryCount: input.Infantry,
            artilleryCount: input.Artillery,
            tankCount: input.Tank,
            fighterCount: input.Fighter,
            bomberCount: input.Bomber,
            antiAirCount: input.AntiAir,
            cruiserCount: input.Cruiser,
            battleshipCount: input.Battleship
        );
    }

    public static NavalArmada CreateNavalArmada(OutcomeUnitsInput input, bool isAttacking)
    {
        return new NavalArmada(
            isAttacking: isAttacking,
            transportCount: input.Transport,
            submarineCount: input.Submarine,
            destroyerCount: input.Destroyer,
            cruiserCount: input.Cruiser,
            battleshipCount: input.Battleship,
            carrierCount: input.Carrier,
            fighterCount: input.Fighter,
            bomberCount: input.Bomber
        );
    }

    public static string? ValidateLandOutcome(
        OutcomeUnitsInput attacker,
        OutcomeUnitsInput defender,
        Units startingAttacker,
        Units startingDefender
    )
    {
        var errors = new List<string>();

        CheckRange(
            "Attacker Infantry",
            attacker.Infantry,
            startingAttacker.InfantryUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Artillery",
            attacker.Artillery,
            startingAttacker.ArtilleryUnits.Count,
            errors
        );
        CheckRange("Attacker Tank", attacker.Tank, startingAttacker.TankUnits.Count, errors);
        CheckRange(
            "Attacker Fighter",
            attacker.Fighter,
            startingAttacker.FighterUnits.Count,
            errors
        );
        CheckRange("Attacker Bomber", attacker.Bomber, startingAttacker.BomberUnits.Count, errors);
        CheckRange(
            "Attacker Anti-Air",
            attacker.AntiAir,
            startingAttacker.AntiAirUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Cruiser",
            attacker.Cruiser,
            startingAttacker.CruiserUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Battleship",
            attacker.Battleship,
            startingAttacker.BattleshipUnits.Count,
            errors
        );

        CheckRange(
            "Defender Infantry",
            defender.Infantry,
            startingDefender.InfantryUnits.Count,
            errors
        );
        CheckRange(
            "Defender Artillery",
            defender.Artillery,
            startingDefender.ArtilleryUnits.Count,
            errors
        );
        CheckRange("Defender Tank", defender.Tank, startingDefender.TankUnits.Count, errors);
        CheckRange(
            "Defender Fighter",
            defender.Fighter,
            startingDefender.FighterUnits.Count,
            errors
        );
        CheckRange("Defender Bomber", defender.Bomber, startingDefender.BomberUnits.Count, errors);
        CheckRange(
            "Defender Anti-Air",
            defender.AntiAir,
            startingDefender.AntiAirUnits.Count,
            errors
        );
        CheckRange(
            "Defender Cruiser",
            defender.Cruiser,
            startingDefender.CruiserUnits.Count,
            errors
        );
        CheckRange(
            "Defender Battleship",
            defender.Battleship,
            startingDefender.BattleshipUnits.Count,
            errors
        );

        ValidateCompletedBattle(TotalLandUnits(attacker), TotalLandUnits(defender), errors);

        return errors.Count == 0 ? null : string.Join(" ", errors);
    }

    public static string? ValidateNavalOutcome(
        OutcomeUnitsInput attacker,
        OutcomeUnitsInput defender,
        Units startingAttacker,
        Units startingDefender
    )
    {
        var errors = new List<string>();

        CheckRange(
            "Attacker Transport",
            attacker.Transport,
            startingAttacker.TransportUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Submarine",
            attacker.Submarine,
            startingAttacker.SubmarineUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Destroyer",
            attacker.Destroyer,
            startingAttacker.DestroyerUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Cruiser",
            attacker.Cruiser,
            startingAttacker.CruiserUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Battleship",
            attacker.Battleship,
            startingAttacker.BattleshipUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Carrier",
            attacker.Carrier,
            startingAttacker.AircraftCarrierUnits.Count,
            errors
        );
        CheckRange(
            "Attacker Fighter",
            attacker.Fighter,
            startingAttacker.FighterUnits.Count,
            errors
        );
        CheckRange("Attacker Bomber", attacker.Bomber, startingAttacker.BomberUnits.Count, errors);

        CheckRange(
            "Defender Transport",
            defender.Transport,
            startingDefender.TransportUnits.Count,
            errors
        );
        CheckRange(
            "Defender Submarine",
            defender.Submarine,
            startingDefender.SubmarineUnits.Count,
            errors
        );
        CheckRange(
            "Defender Destroyer",
            defender.Destroyer,
            startingDefender.DestroyerUnits.Count,
            errors
        );
        CheckRange(
            "Defender Cruiser",
            defender.Cruiser,
            startingDefender.CruiserUnits.Count,
            errors
        );
        CheckRange(
            "Defender Battleship",
            defender.Battleship,
            startingDefender.BattleshipUnits.Count,
            errors
        );
        CheckRange(
            "Defender Carrier",
            defender.Carrier,
            startingDefender.AircraftCarrierUnits.Count,
            errors
        );
        CheckRange(
            "Defender Fighter",
            defender.Fighter,
            startingDefender.FighterUnits.Count,
            errors
        );
        CheckRange("Defender Bomber", defender.Bomber, startingDefender.BomberUnits.Count, errors);

        ValidateCompletedBattle(TotalNavalUnits(attacker), TotalNavalUnits(defender), errors);

        return errors.Count == 0 ? null : string.Join(" ", errors);
    }

    public static List<LuckyMetricRow> BuildLuckyMetricRows(LuckyStats luckyStats)
    {
        return
        [
            new LuckyMetricRow(
                "IPC Attacker Luck",
                $"{luckyStats.ipcAttackerLuck:F2}",
                "Shows how much IPC the attacker 'saved' compared to the median result. It is the difference between the actual attacker remaining IPC and the average simulated attacker remaining IPC. Positive means the attacker finished better than average."
            ),
            new LuckyMetricRow(
                "IPC Defender Luck",
                $"{luckyStats.ipcDefenderLuck:F2}",
                "Shows how much IPC the defender 'saved' compared to the median result. it is the difference between the actual defender remaining IPC and the average simulated defender remaining IPC. Positive means the defender finished better than average."
            ),
            new LuckyMetricRow(
                "Percentile",
                $"{luckyStats.percentile:P2}",
                "It's the probability mass of simulated outcomes that were strictly worse (for the attacker) than the actual result as visualised by the highlighted area of the graph. 50% mean the outcome was equally favourable to both sides. Higher percentile means the real outcome was more favorable for the attacker."
            ),
            new LuckyMetricRow(
                "Shock",
                $"{luckyStats.shock:F2} bits",
                "Shock is -log2(P) for the exact outcome. Higher shock means the exact result was rare. It is also how many consequent heads or tails coinflips one would have to make to achieve this outcome."
            ),
        ];
    }

    public static string FormatOutcomeStatus(int attackerRemainingCost, int defenderRemainingCost)
    {
        var delta = attackerRemainingCost - defenderRemainingCost;
        return $"Actual IPC delta: {delta:+#;-#;0}";
    }

    private static void CheckRange(string label, int actual, int maximum, List<string> errors)
    {
        if (actual < 0)
        {
            errors.Add($"{label} cannot be negative.");
        }
        else if (actual > maximum)
        {
            errors.Add($"{label} cannot exceed {maximum}.");
        }
    }

    private static void ValidateCompletedBattle(
        int attackerRemainingCount,
        int defenderRemainingCount,
        List<string> errors
    )
    {
        if (attackerRemainingCount > 0 && defenderRemainingCount > 0)
        {
            errors.Add(
                "A completed battle cannot leave both attacker and defender with participating units remaining."
            );
        }
    }

    private static int TotalLandUnits(OutcomeUnitsInput input) =>
        input.Infantry
        + input.Artillery
        + input.Tank
        + input.Fighter
        + input.Bomber
        + input.AntiAir
        + input.Cruiser
        + input.Battleship;

    private static int TotalNavalUnits(OutcomeUnitsInput input) =>
        input.Transport
        + input.Submarine
        + input.Destroyer
        + input.Cruiser
        + input.Battleship
        + input.Carrier
        + input.Fighter
        + input.Bomber;

    private static int Clamp(int value) => Math.Max(0, value);
}
