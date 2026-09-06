using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using axis_console_project.Armies;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Sea;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

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
    [ObservableProperty] private List<LuckyMetricRow> _luckyMetricRows = [];
    [ObservableProperty] private string _actualOutcomeStatusMessage = "Enter the actual remaining units, then analyze the result.";
    [ObservableProperty] private ISeries[] _probabilityDistributionSeries = [];
    [ObservableProperty] private Axis[] _probabilityDistributionXAxes = [];
    [ObservableProperty] private Axis[] _probabilityDistributionYAxes = [];

    private CancellationTokenSource? _cancellationTokenSource;
    private SimulationStats? _lastSimulationStats;
    private int? _highlightedActualResultValue;

    public OutcomeUnitsInput ActualAttackerRemaining { get; } = new();
    public OutcomeUnitsInput ActualDefenderRemaining { get; } = new();

    public bool HasResults => BattleOutcomeRows.Count > 0;
    public bool HasLuckyStats => LuckyMetricRows.Count > 0;

    public int AttackerCost => CreateArmada(true).Cost;
    public int DefenderCost => CreateArmada(false).Cost;

    [RelayCommand]
    private void IncrementUnit(string key) => AdjustUnit(key, 1);

    [RelayCommand]
    private void DecrementUnit(string key) => AdjustUnit(key, -1);

    [RelayCommand]
    private void IncrementActualOutcomeUnit(string key) => AdjustActualOutcomeUnit(key, 1);

    [RelayCommand]
    private void DecrementActualOutcomeUnit(string key) => AdjustActualOutcomeUnit(key, -1);

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

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        IsBusy = true;
        StatusMessage = "Running naval simulation...";

        try
        {
            var attacking = CreateArmada(true);
            var defending = CreateArmada(false);

            var stats = await Task.Run(() =>
            {
                var simulation = new Simulation(attacking, defending);
                simulation.Run(SimulationCount, cancellationToken: cancellationToken);
                return simulation.Stats;
            }, cancellationToken);

            _lastSimulationStats = stats;
            ResetActualOutcomeAnalysisState();
            PopulateResultsTables(stats);
            StatusMessage = "Naval simulation complete.";
            OnPropertyChanged(nameof(AttackerCost));
            OnPropertyChanged(nameof(DefenderCost));
            OnPropertyChanged(nameof(HasResults));

            await Shell.Current.GoToAsync("//naval/naval-results-tab");
        }
        catch (OperationCanceledException)
        {
            StatusMessage = "Simulation canceled.";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Simulation failed: {ex.Message}";
            BattleOutcomeRows = [];
            ArmyCompositionRows = [];
            RemainingUnitsRows = [];
            SummaryMetricRows = [];
            _lastSimulationStats = null;
            ResetActualOutcomeAnalysisState();
            UpdateProbabilityChart();
            OnPropertyChanged(nameof(HasResults));
        }
        finally
        {
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            IsBusy = false;
        }
    }

    [RelayCommand]
    private void CancelSimulation()
    {
        if (!IsBusy)
        {
            return;
        }

        StatusMessage = "Canceling simulation...";
        _cancellationTokenSource?.Cancel();
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

    [RelayCommand]
    private void Reset()
    {
        AttackerTransport = 0;
        AttackerSubmarine = 0;
        AttackerDestroyer = 0;
        AttackerCruiser = 0;
        AttackerBattleship = 0;
        AttackerCarrier = 0;
        AttackerFighter = 0;
        AttackerBomber = 0;

        DefenderTransport = 0;
        DefenderSubmarine = 0;
        DefenderDestroyer = 0;
        DefenderCruiser = 0;
        DefenderBattleship = 0;
        DefenderCarrier = 0;
        DefenderFighter = 0;
        DefenderBomber = 0;

        OnPropertyChanged(nameof(AttackerCost));
        OnPropertyChanged(nameof(DefenderCost));
        StatusMessage = "All naval units reset to zero.";
        ResetActualOutcomeAnalysisState();
    }

    [RelayCommand]
    private void AnalyzeActualOutcome()
    {
        if (_lastSimulationStats?.AttackingArmy is not NavalArmada attackingArmada ||
            _lastSimulationStats.DefendingArmy is not NavalArmada defendingArmada)
        {
            ActualOutcomeStatusMessage = "Run a simulation first.";
            return;
        }

        var validationError = ActualOutcomeAnalysisHelper.ValidateNavalOutcome(
            ActualAttackerRemaining,
            ActualDefenderRemaining,
            attackingArmada.Units,
            defendingArmada.Units
        );

        if (validationError is not null)
        {
            LuckyMetricRows = [];
            _highlightedActualResultValue = null;
            ActualOutcomeStatusMessage = validationError;
            OnPropertyChanged(nameof(HasLuckyStats));
            UpdateProbabilityChart();
            return;
        }

        var actualAttackerArmada = ActualOutcomeAnalysisHelper.CreateNavalArmada(ActualAttackerRemaining, true);
        var actualDefenderArmada = ActualOutcomeAnalysisHelper.CreateNavalArmada(ActualDefenderRemaining, false);

        try
        {
            var luckyStats = _lastSimulationStats.HowLuckyWasThisOutcome(
                actualAttackerArmada.Units,
                actualDefenderArmada.Units
            );

            _highlightedActualResultValue = actualAttackerArmada.Cost - actualDefenderArmada.Cost;
            LuckyMetricRows = ActualOutcomeAnalysisHelper.BuildLuckyMetricRows(luckyStats);
            ActualOutcomeStatusMessage = ActualOutcomeAnalysisHelper.FormatOutcomeStatus(
                actualAttackerArmada.Cost,
                actualDefenderArmada.Cost
            );
            OnPropertyChanged(nameof(HasLuckyStats));
            UpdateProbabilityChart();
        }
        catch (Exception ex)
        {
            LuckyMetricRows = [];
            _highlightedActualResultValue = null;
            ActualOutcomeStatusMessage = ex.Message;
            OnPropertyChanged(nameof(HasLuckyStats));
            UpdateProbabilityChart();
        }
    }

    [RelayCommand]
    private void ClearActualOutcome()
    {
        ResetActualOutcomeAnalysisState();
        UpdateProbabilityChart();
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

    private void AdjustActualOutcomeUnit(string key, int delta)
    {
        var parts = key.Split('_', 2);
        if (parts.Length != 2)
        {
            return;
        }

        var target = parts[0] == "attacker" ? ActualAttackerRemaining : ActualDefenderRemaining;
        ActualOutcomeAnalysisHelper.AdjustUnitCount(target, parts[1], delta);
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
        _lastSimulationStats = stats;

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
            new MetricRow("Average Attacker IPC Loss", $"{stats.AttackerAvgCpLoss:F2}"),
            new MetricRow("Average Defender IPC Loss", $"{stats.DefenderAvgCpLoss:F2}"),
            new MetricRow("Cost Comparison", $"{stats.AttackingArmy?.Cost ?? 0} IPC vs {stats.DefendingArmy?.Cost ?? 0} IPC")
        ];

        UpdateProbabilityChart();
    }

    private void ResetActualOutcomeAnalysisState()
    {
        ActualAttackerRemaining.Reset();
        ActualDefenderRemaining.Reset();
        LuckyMetricRows = [];
        ActualOutcomeStatusMessage = "Enter the actual remaining units, then analyze the result.";
        _highlightedActualResultValue = null;
        OnPropertyChanged(nameof(HasLuckyStats));
    }

    private void UpdateProbabilityChart()
    {
        var chartData = ProbabilityDistributionChartBuilder.Build(
            _lastSimulationStats,
            _highlightedActualResultValue
        );
        ProbabilityDistributionSeries = chartData.Series;
        ProbabilityDistributionXAxes = chartData.XAxes;
        ProbabilityDistributionYAxes = chartData.YAxes;
    }

    private static int Clamp(int value) => Math.Max(0, value);
}

