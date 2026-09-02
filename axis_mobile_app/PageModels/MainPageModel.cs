using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using axis_console_project.Armies;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

namespace axis_mobile_app.PageModels;

public partial class MainPageModel : ObservableObject
{
    [ObservableProperty] private int _attackerInfantry;
    [ObservableProperty] private int _attackerArtillery;
    [ObservableProperty] private int _attackerTank;
    [ObservableProperty] private int _attackerFighter;
    [ObservableProperty] private int _attackerBomber;
    [ObservableProperty] private int _attackerAntiAir;
    [ObservableProperty] private int _attackerCruiser;
    [ObservableProperty] private int _attackerBattleship;

    [ObservableProperty] private int _defenderInfantry;
    [ObservableProperty] private int _defenderArtillery;
    [ObservableProperty] private int _defenderTank;
    [ObservableProperty] private int _defenderFighter;
    [ObservableProperty] private int _defenderBomber;
    [ObservableProperty] private int _defenderAntiAir;
    [ObservableProperty] private int _defenderCruiser;
    [ObservableProperty] private int _defenderBattleship;

    [ObservableProperty] private int _simulationCount = 10000;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _statusMessage = "Configure armies and run a simulation.";
    [ObservableProperty] private string _resultsText = string.Empty;
    [ObservableProperty] private List<OutcomeRow> _battleOutcomeRows = [];
    [ObservableProperty] private List<ComparisonRow> _armyCompositionRows = [];
    [ObservableProperty] private List<ComparisonRow> _remainingUnitsRows = [];
    [ObservableProperty] private List<MetricRow> _summaryMetricRows = [];

    public bool HasResults => BattleOutcomeRows.Count > 0;

    public int AttackerCost => CreateArmy(isAttacking: true).Cost;
    public int DefenderCost => CreateArmy(isAttacking: false).Cost;

    [RelayCommand]
    private void IncrementUnit(string key) => AdjustUnit(key, 1);

    [RelayCommand]
    private void DecrementUnit(string key) => AdjustUnit(key, -1);

    [RelayCommand]
    private void IncrementSimulationCount()
    {
        SimulationCount += 1000;
    }

    [RelayCommand]
    private void DecrementSimulationCount()
    {
        SimulationCount = Math.Max(100, SimulationCount - 1000);
    }

    [RelayCommand]
    private async Task RunSimulation()
    {
        if (IsBusy)
        {
            return;
        }

        if (SimulationCount <= 0)
        {
            StatusMessage = "Simulation count must be greater than zero.";
            return;
        }

        IsBusy = true;
        StatusMessage = "Running simulation...";

        try
        {
            var attackingArmy = CreateArmy(isAttacking: true);
            var defendingArmy = CreateArmy(isAttacking: false);

            var stats = await Task.Run(() =>
            {
                var simulation = new Simulation(attackingArmy, defendingArmy);
                simulation.Run(SimulationCount);
                return simulation.Stats;
            });

            PopulateResultsTables(stats);
            ResultsText = BuildResultsText(stats);
            StatusMessage = "Simulation complete.";
            OnPropertyChanged(nameof(AttackerCost));
            OnPropertyChanged(nameof(DefenderCost));
            OnPropertyChanged(nameof(HasResults));

            await Shell.Current.GoToAsync("//results");
        }
        catch (Exception ex)
        {
            StatusMessage = "Simulation failed.";
            ResultsText = ex.Message;
            BattleOutcomeRows = [];
            ArmyCompositionRows = [];
            RemainingUnitsRows = [];
            SummaryMetricRows = [];
            OnPropertyChanged(nameof(HasResults));
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    private async Task ViewResults()
    {
        if (!HasResults)
        {
            StatusMessage = "Run a simulation first.";
            return;
        }

        await Shell.Current.GoToAsync("//results");
    }

    private void AdjustUnit(string key, int delta)
    {
        switch (key)
        {
            case "attacker_infantry":
                AttackerInfantry = Clamp(AttackerInfantry + delta);
                break;
            case "attacker_artillery":
                AttackerArtillery = Clamp(AttackerArtillery + delta);
                break;
            case "attacker_tank":
                AttackerTank = Clamp(AttackerTank + delta);
                break;
            case "attacker_fighter":
                AttackerFighter = Clamp(AttackerFighter + delta);
                break;
            case "attacker_bomber":
                AttackerBomber = Clamp(AttackerBomber + delta);
                break;
            case "attacker_antiair":
                AttackerAntiAir = Clamp(AttackerAntiAir + delta);
                break;
            case "attacker_cruiser":
                AttackerCruiser = Clamp(AttackerCruiser + delta);
                break;
            case "attacker_battleship":
                AttackerBattleship = Clamp(AttackerBattleship + delta);
                break;
            case "defender_infantry":
                DefenderInfantry = Clamp(DefenderInfantry + delta);
                break;
            case "defender_artillery":
                DefenderArtillery = Clamp(DefenderArtillery + delta);
                break;
            case "defender_tank":
                DefenderTank = Clamp(DefenderTank + delta);
                break;
            case "defender_fighter":
                DefenderFighter = Clamp(DefenderFighter + delta);
                break;
            case "defender_bomber":
                DefenderBomber = Clamp(DefenderBomber + delta);
                break;
            case "defender_antiair":
                DefenderAntiAir = Clamp(DefenderAntiAir + delta);
                break;
            case "defender_cruiser":
                DefenderCruiser = Clamp(DefenderCruiser + delta);
                break;
            case "defender_battleship":
                DefenderBattleship = Clamp(DefenderBattleship + delta);
                break;
        }

        OnPropertyChanged(nameof(AttackerCost));
        OnPropertyChanged(nameof(DefenderCost));
    }

    private static int Clamp(int value) => Math.Max(0, value);

    private LandArmy CreateArmy(bool isAttacking)
    {
        if (isAttacking)
        {
            return new LandArmy(
                isAttacking: true,
                infantryCount: AttackerInfantry,
                artilleryCount: AttackerArtillery,
                tankCount: AttackerTank,
                fighterCount: AttackerFighter,
                bomberCount: AttackerBomber,
                antiAirCount: AttackerAntiAir,
                cruiserCount: AttackerCruiser,
                battleshipCount: AttackerBattleship
            );
        }

        return new LandArmy(
            isAttacking: false,
            infantryCount: DefenderInfantry,
            artilleryCount: DefenderArtillery,
            tankCount: DefenderTank,
            fighterCount: DefenderFighter,
            bomberCount: DefenderBomber,
            antiAirCount: DefenderAntiAir,
            cruiserCount: DefenderCruiser,
            battleshipCount: DefenderBattleship
        );
    }

    private void PopulateResultsTables(SimulationStats stats)
    {
        var attackingUnits = stats.AttackingArmy?.Units ?? new Units(false);
        var defendingUnits = stats.DefendingArmy?.Units ?? new Units(false);

        BattleOutcomeRows =
        [
            new OutcomeRow("Attacker Wins", stats.AttackerWon, $"{stats.AttackerWonPercentage:F2}%"),
            new OutcomeRow("Defender Wins", stats.DefenderWon, $"{stats.DefenderWonPercentage:F2}%"),
            new OutcomeRow("Draws", stats.Draw, $"{stats.DrawPercentage:F2}%")
        ];

        ArmyCompositionRows =
        [
            new ComparisonRow("Infantry", attackingUnits.InfantryUnits.Count.ToString(), defendingUnits.InfantryUnits.Count.ToString()),
            new ComparisonRow("Artillery", attackingUnits.ArtilleryUnits.Count.ToString(), defendingUnits.ArtilleryUnits.Count.ToString()),
            new ComparisonRow("Tank", attackingUnits.TankUnits.Count.ToString(), defendingUnits.TankUnits.Count.ToString()),
            new ComparisonRow("Fighter", attackingUnits.FighterUnits.Count.ToString(), defendingUnits.FighterUnits.Count.ToString()),
            new ComparisonRow("Bomber", attackingUnits.BomberUnits.Count.ToString(), defendingUnits.BomberUnits.Count.ToString()),
            new ComparisonRow("Anti-Air", attackingUnits.AntiAirUnits.Count.ToString(), defendingUnits.AntiAirUnits.Count.ToString()),
            new ComparisonRow("Cruiser", attackingUnits.CruiserUnits.Count.ToString(), defendingUnits.CruiserUnits.Count.ToString()),
            new ComparisonRow("Battleship", attackingUnits.BattleshipUnits.Count.ToString(), defendingUnits.BattleshipUnits.Count.ToString())
        ];

        RemainingUnitsRows =
        [
            new ComparisonRow("Infantry", $"{stats.AttackerRemainingUnitsAvg.InfantryUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.InfantryUnits:F2}"),
            new ComparisonRow("Artillery", $"{stats.AttackerRemainingUnitsAvg.ArtilleryUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.ArtilleryUnits:F2}"),
            new ComparisonRow("Tank", $"{stats.AttackerRemainingUnitsAvg.TankUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.TankUnits:F2}"),
            new ComparisonRow("Fighter", $"{stats.AttackerRemainingUnitsAvg.FighterUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.FighterUnits:F2}"),
            new ComparisonRow("Bomber", $"{stats.AttackerRemainingUnitsAvg.BomberUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.BomberUnits:F2}"),
            new ComparisonRow("Anti-Air", $"{stats.AttackerRemainingUnitsAvg.AntiAirUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.AntiAirUnits:F2}"),
            new ComparisonRow("Cruiser", $"{stats.AttackerRemainingUnitsAvg.CruiserUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.CruiserUnits:F2}"),
            new ComparisonRow("Battleship", $"{stats.AttackerRemainingUnitsAvg.BattleshipUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.BattleshipUnits:F2}")
        ];

        SummaryMetricRows =
        [
            new MetricRow("Total Battles", stats.TotalBattles.ToString()),
            new MetricRow("Average Attacker CP Loss", $"{stats.AttackerAvgCpLoss:F2}"),
            new MetricRow("Average Defender CP Loss", $"{stats.DefenderAvgCpLoss:F2}"),
            new MetricRow("Cost Comparison", $"{stats.AttackingArmy?.Cost ?? 0} CP vs {stats.DefendingArmy?.Cost ?? 0} CP")
        ];
    }

    private static string BuildResultsText(SimulationStats stats)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine("Battle Results:");
        sb.AppendLine($"Attacker Wins: {stats.AttackerWon}, {stats.AttackerWonPercentage:F2}%");
        sb.AppendLine($"Defender Wins: {stats.DefenderWon}, {stats.DefenderWonPercentage:F2}%");
        sb.AppendLine($"Draws: {stats.Draw}, {stats.DrawPercentage:F2}%");
        sb.AppendLine();
        sb.AppendLine("Attacker Army:");
        sb.AppendLine($"{stats.AttackingArmy?.Units}");
        sb.AppendLine("Defending Army:");
        sb.AppendLine($"{stats.DefendingArmy?.Units}");
        sb.AppendLine();
        sb.AppendLine("Average Attacker Remaining Units:");
        sb.AppendLine($"{stats.AttackerRemainingUnitsAvg}");
        sb.AppendLine("Average Defender Remaining Units:");
        sb.AppendLine($"{stats.DefenderRemainingUnitsAvg}");
        sb.AppendLine($"Average Attacker CP Loss: {stats.AttackerAvgCpLoss:F2}");
        sb.AppendLine($"Average Defender CP Loss: {stats.DefenderAvgCpLoss:F2}");
        sb.AppendLine($"{stats.AttackingArmy?.Cost ?? 0} CP vs {stats.DefendingArmy?.Cost ?? 0} CP");

        return sb.ToString();
    }
}

public record OutcomeRow(string Result, int Count, string Percentage);

public record ComparisonRow(string Unit, string Attacker, string Defender);

public record MetricRow(string Metric, string Value);

