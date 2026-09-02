using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using axis_console_project.Armies;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Sea;

namespace axis_mobile_app.PageModels;

public partial class NavalPageModel : ObservableObject
{
    [ObservableProperty] private int _attackerTransport;
    [ObservableProperty] private int _attackerSubmarine;
    [ObservableProperty] private int _attackerDestroyer;
    [ObservableProperty] private int _attackerCruiser;
    [ObservableProperty] private int _attackerBattleship;
    [ObservableProperty] private int _attackerCarrier;
    [ObservableProperty] private int _attackerFighter;
    [ObservableProperty] private int _attackerBomber;

    [ObservableProperty] private int _defenderTransport;
    [ObservableProperty] private int _defenderSubmarine;
    [ObservableProperty] private int _defenderDestroyer;
    [ObservableProperty] private int _defenderCruiser;
    [ObservableProperty] private int _defenderBattleship;
    [ObservableProperty] private int _defenderCarrier;
    [ObservableProperty] private int _defenderFighter;
    [ObservableProperty] private int _defenderBomber;

    [ObservableProperty] private int _simulationCount = 10000;
    [ObservableProperty] private bool _isBusy;
    [ObservableProperty] private string _statusMessage = "Configure navies and run a simulation.";
    [ObservableProperty] private List<OutcomeRow> _battleOutcomeRows = [];
    [ObservableProperty] private List<ComparisonRow> _armyCompositionRows = [];
    [ObservableProperty] private List<ComparisonRow> _remainingUnitsRows = [];
    [ObservableProperty] private List<MetricRow> _summaryMetricRows = [];

    public bool HasResults => BattleOutcomeRows.Count > 0;

    public int AttackerCost => CreateArmada(true).Cost;
    public int DefenderCost => CreateArmada(false).Cost;

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
        StatusMessage = "Running naval simulation...";

        try
        {
            var attacking = CreateArmada(true);
            var defending = CreateArmada(false);

            var stats = await Task.Run(() =>
            {
                var simulation = new Simulation(attacking, defending);
                simulation.Run(SimulationCount);
                return simulation.Stats;
            });

            PopulateResultsTables(stats);
            StatusMessage = "Naval simulation complete.";
            OnPropertyChanged(nameof(AttackerCost));
            OnPropertyChanged(nameof(DefenderCost));
            OnPropertyChanged(nameof(HasResults));

            await Shell.Current.GoToAsync("//naval/naval-results-tab");
        }
        catch (Exception ex)
        {
            StatusMessage = $"Simulation failed: {ex.Message}";
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

        await Shell.Current.GoToAsync("//naval/naval-results-tab");
    }

    private void AdjustUnit(string key, int delta)
    {
        switch (key)
        {
            case "attacker_transport": AttackerTransport = Clamp(AttackerTransport + delta); break;
            case "attacker_submarine": AttackerSubmarine = Clamp(AttackerSubmarine + delta); break;
            case "attacker_destroyer": AttackerDestroyer = Clamp(AttackerDestroyer + delta); break;
            case "attacker_cruiser": AttackerCruiser = Clamp(AttackerCruiser + delta); break;
            case "attacker_battleship": AttackerBattleship = Clamp(AttackerBattleship + delta); break;
            case "attacker_carrier": AttackerCarrier = Clamp(AttackerCarrier + delta); break;
            case "attacker_fighter": AttackerFighter = Clamp(AttackerFighter + delta); break;
            case "attacker_bomber": AttackerBomber = Clamp(AttackerBomber + delta); break;

            case "defender_transport": DefenderTransport = Clamp(DefenderTransport + delta); break;
            case "defender_submarine": DefenderSubmarine = Clamp(DefenderSubmarine + delta); break;
            case "defender_destroyer": DefenderDestroyer = Clamp(DefenderDestroyer + delta); break;
            case "defender_cruiser": DefenderCruiser = Clamp(DefenderCruiser + delta); break;
            case "defender_battleship": DefenderBattleship = Clamp(DefenderBattleship + delta); break;
            case "defender_carrier": DefenderCarrier = Clamp(DefenderCarrier + delta); break;
            case "defender_fighter": DefenderFighter = Clamp(DefenderFighter + delta); break;
            case "defender_bomber": DefenderBomber = Clamp(DefenderBomber + delta); break;
        }

        OnPropertyChanged(nameof(AttackerCost));
        OnPropertyChanged(nameof(DefenderCost));
    }

    private NavalArmada CreateArmada(bool isAttacking)
    {
        if (isAttacking)
        {
            return new NavalArmada(
                isAttacking: true,
                transportCount: AttackerTransport,
                submarineCount: AttackerSubmarine,
                destroyerCount: AttackerDestroyer,
                cruiserCount: AttackerCruiser,
                battleshipCount: AttackerBattleship,
                carrierCount: AttackerCarrier,
                fighterCount: AttackerFighter,
                bomberCount: AttackerBomber
            );
        }

        return new NavalArmada(
            isAttacking: false,
            transportCount: DefenderTransport,
            submarineCount: DefenderSubmarine,
            destroyerCount: DefenderDestroyer,
            cruiserCount: DefenderCruiser,
            battleshipCount: DefenderBattleship,
            carrierCount: DefenderCarrier,
            fighterCount: DefenderFighter,
            bomberCount: DefenderBomber
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
            new ComparisonRow("Transport", attackingUnits.TransportUnits.Count.ToString(), defendingUnits.TransportUnits.Count.ToString()),
            new ComparisonRow("Submarine", attackingUnits.SubmarineUnits.Count.ToString(), defendingUnits.SubmarineUnits.Count.ToString()),
            new ComparisonRow("Destroyer", attackingUnits.DestroyerUnits.Count.ToString(), defendingUnits.DestroyerUnits.Count.ToString()),
            new ComparisonRow("Cruiser", attackingUnits.CruiserUnits.Count.ToString(), defendingUnits.CruiserUnits.Count.ToString()),
            new ComparisonRow("Battleship", attackingUnits.BattleshipUnits.Count.ToString(), defendingUnits.BattleshipUnits.Count.ToString()),
            new ComparisonRow("Carrier", attackingUnits.AircraftCarrierUnits.Count.ToString(), defendingUnits.AircraftCarrierUnits.Count.ToString()),
            new ComparisonRow("Fighter", attackingUnits.FighterUnits.Count.ToString(), defendingUnits.FighterUnits.Count.ToString()),
            new ComparisonRow("Bomber", attackingUnits.BomberUnits.Count.ToString(), defendingUnits.BomberUnits.Count.ToString())
        ];

        RemainingUnitsRows =
        [
            new ComparisonRow("Transport", $"{stats.AttackerRemainingUnitsAvg.TransportUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.TransportUnits:F2}"),
            new ComparisonRow("Submarine", $"{stats.AttackerRemainingUnitsAvg.SubmarineUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.SubmarineUnits:F2}"),
            new ComparisonRow("Destroyer", $"{stats.AttackerRemainingUnitsAvg.DestroyerUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.DestroyerUnits:F2}"),
            new ComparisonRow("Cruiser", $"{stats.AttackerRemainingUnitsAvg.CruiserUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.CruiserUnits:F2}"),
            new ComparisonRow("Battleship", $"{stats.AttackerRemainingUnitsAvg.BattleshipUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.BattleshipUnits:F2}"),
            new ComparisonRow("Carrier", $"{stats.AttackerRemainingUnitsAvg.AircraftCarrierUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.AircraftCarrierUnits:F2}"),
            new ComparisonRow("Fighter", $"{stats.AttackerRemainingUnitsAvg.FighterUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.FighterUnits:F2}"),
            new ComparisonRow("Bomber", $"{stats.AttackerRemainingUnitsAvg.BomberUnits:F2}", $"{stats.DefenderRemainingUnitsAvg.BomberUnits:F2}")
        ];

        SummaryMetricRows =
        [
            new MetricRow("Total Battles", stats.TotalBattles.ToString()),
            new MetricRow("Average Attacker CP Loss", $"{stats.AttackerAvgCpLoss:F2}"),
            new MetricRow("Average Defender CP Loss", $"{stats.DefenderAvgCpLoss:F2}"),
            new MetricRow("Cost Comparison", $"{stats.AttackingArmy?.Cost ?? 0} CP vs {stats.DefendingArmy?.Cost ?? 0} CP")
        ];
    }

    private static int Clamp(int value) => Math.Max(0, value);
}

