using axis_console_project.Armies;
using axis_console_project.Resolvers;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace axis_mobile_app.PageModels;

public partial class CounterPageModel : ObservableObject
{
    public IReadOnlyList<string> ArmyTypeOptions { get; } = ["Land", "Naval"];
    public IReadOnlyList<string> TargetRoleOptions { get; } = ["Attacking", "Defending"];

    [ObservableProperty]
    private string _selectedArmyType = "Land";

    [ObservableProperty]
    private string _selectedTargetRole = "Attacking";

    [ObservableProperty]
    private int _landInfantry;

    [ObservableProperty]
    private int _landArtillery;

    [ObservableProperty]
    private int _landTank;

    [ObservableProperty]
    private int _landFighter;

    [ObservableProperty]
    private int _landBomber;

    [ObservableProperty]
    private int _landAntiAir;

    [ObservableProperty]
    private int _landCruiser;

    [ObservableProperty]
    private int _landBattleship;

    [ObservableProperty]
    private int _navalTransport;

    [ObservableProperty]
    private int _navalSubmarine;

    [ObservableProperty]
    private int _navalDestroyer;

    [ObservableProperty]
    private int _navalCruiser;

    [ObservableProperty]
    private int _navalBattleship;

    [ObservableProperty]
    private int _navalCarrier;

    [ObservableProperty]
    private int _navalFighter;

    [ObservableProperty]
    private int _navalBomber;

    [ObservableProperty]
    private int _simulationCount = 1000;

    [ObservableProperty]
    private string _budgetOverrideText = string.Empty;

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private double _counterSearchProgress;

    [ObservableProperty]
    private string _statusMessage = "Configure a target army and run a counter search.";

    [ObservableProperty]
    private string _targetArmySummaryText = string.Empty;

    [ObservableProperty]
    private string _targetArmyCompositionText = string.Empty;

    [ObservableProperty]
    private string _targetArmyCostText = string.Empty;

    [ObservableProperty]
    private string _bestCounterArmySummaryText = string.Empty;

    [ObservableProperty]
    private string _bestCounterArmyCompositionText = string.Empty;

    [ObservableProperty]
    private string _bestCounterArmyCostText = string.Empty;

    [ObservableProperty]
    private string _resultsSummaryText = string.Empty;

    [ObservableProperty]
    private string _actualOutcomeStatusMessage =
        "Enter the actual remaining units, then analyze the result.";

    [ObservableProperty]
    private List<OutcomeRow> _battleOutcomeRows = [];

    [ObservableProperty]
    private List<ComparisonRow> _armyCompositionRows = [];

    [ObservableProperty]
    private List<ComparisonRow> _remainingUnitsRows = [];

    [ObservableProperty]
    private List<MetricRow> _summaryMetricRows = [];

    [ObservableProperty]
    private List<LuckyMetricRow> _luckyMetricRows = [];

    [ObservableProperty]
    private ISeries[] _probabilityDistributionSeries = [];

    [ObservableProperty]
    private Axis[] _probabilityDistributionXAxes = [];

    [ObservableProperty]
    private Axis[] _probabilityDistributionYAxes = [];

    private bool _budgetOverrideIsCustom;
    private Army? _lastBestCounterArmy;
    private CancellationTokenSource? _cancellationTokenSource;
    private SimulationStats? _lastMatchupStats;
    private int? _highlightedActualResultValue;

    public OutcomeUnitsInput ActualAttackerRemaining { get; } = new();
    public OutcomeUnitsInput ActualDefenderRemaining { get; } = new();

    public CounterPageModel()
    {
        BudgetOverrideText = TargetCost.ToString();
    }

    public bool HasResults => BattleOutcomeRows.Count > 0;
    public bool IsLandArmy => SelectedArmyType == ArmyTypeOptions[0];
    public bool IsNavalArmy => !IsLandArmy;
    public bool IsTargetAttacking => SelectedTargetRole == TargetRoleOptions[0];
    public bool IsTargetDefending => !IsTargetAttacking;
    public string TargetArmyTypeLabel => IsLandArmy ? "Land Army" : "Naval Armada";
    public string TargetArmyRoleLabel => IsTargetAttacking ? "Attacking" : "Defending";
    public string TargetCompositionTitle =>
        IsLandArmy ? "Land Army Composition" : "Naval Armada Composition";
    public string TargetArmyHeader => $"Target {TargetArmyTypeLabel} ({TargetArmyRoleLabel})";
    public string BudgetOverridePlaceholder => $"Default: {TargetCost}";
    public string BudgetOverrideSummaryText
    {
        get
        {
            var currentBudget = int.TryParse(BudgetOverrideText.Trim(), out var parsedBudget)
                ? parsedBudget
                : TargetCost;

            var delta = currentBudget - TargetCost;
            var deltaText = delta switch
            {
                > 0 => $"+{delta}",
                < 0 => delta.ToString(),
                _ => "0",
            };

            return $"{currentBudget} IPC ({deltaText} vs target)";
        }
    }
    public string CounterSearchProgressText => $"{CounterSearchProgress:F2}%";
    public double CounterSearchProgressFraction => Math.Clamp(CounterSearchProgress / 100.0, 0, 1);
    public int TargetCost => IsLandArmy ? GetLandTargetCost() : GetNavalTargetCost();
    public int BestCounterCost => _lastBestCounterArmy?.Cost ?? 0;
    public bool HasLuckyStats => LuckyMetricRows.Count > 0;

    [RelayCommand]
    private void IncrementUnit(string key) => AdjustUnit(key, 1);

    [RelayCommand]
    private void DecrementUnit(string key) => AdjustUnit(key, -1);

    [RelayCommand]
    private void IncrementBudgetOverride() => AdjustBudgetOverride(1);

    [RelayCommand]
    private void DecrementBudgetOverride() => AdjustBudgetOverride(-1);

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
    private async Task RunCounterSearch()
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

        if (!TryGetBudgetOverride(out var budget, out var errorMessage))
        {
            StatusMessage = errorMessage;
            return;
        }

        var targetArmy = CreateTargetArmy();
        var builder = CreateBuilder();

        _cancellationTokenSource?.Dispose();
        _cancellationTokenSource = new CancellationTokenSource();
        var cancellationToken = _cancellationTokenSource.Token;

        IsBusy = true;
        CounterSearchProgress = 0;
        StatusMessage = "Searching for the best counter army...";

        try
        {
            IProgress<double> progress = new Progress<double>(value =>
                CounterSearchProgress = value
            );
            var bestCounterArmy = await Task.Run(
                () =>
                    builder.CreateCounterArmy(
                        targetArmy,
                        cost: budget,
                        sims: SimulationCount,
                        verbose: false,
                        progressCallback: progress.Report,
                        cancellationToken: cancellationToken
                    ),
                cancellationToken
            );

            StatusMessage = "Running the final matchup simulation...";
            CounterSearchProgress = 0;

            var matchupStats = await Task.Run(
                () =>
                {
                    var simulation = new Simulation(bestCounterArmy, targetArmy);
                    simulation.Run(SimulationCount, progress.Report, cancellationToken);
                    return simulation.Stats;
                },
                cancellationToken
            );

            ResetActualOutcomeAnalysisState();
            PopulateResults(targetArmy, bestCounterArmy, matchupStats, budget);
            StatusMessage = "Counter search complete.";
            CounterSearchProgress = 100;

            await Shell.Current.GoToAsync("//counter/counter-results-tab");
        }
        catch (OperationCanceledException)
        {
            ResetResults();
            StatusMessage = "Counter search canceled.";
            CounterSearchProgress = 0;
        }
        catch (Exception ex)
        {
            ResetResults();
            StatusMessage = $"Counter search failed: {ex.Message}";
            CounterSearchProgress = 0;
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

        StatusMessage = "Canceling counter search...";
        _cancellationTokenSource?.Cancel();
    }

    [RelayCommand]
    private async Task ViewResults()
    {
        if (!HasResults)
        {
            StatusMessage = "Run a counter search first.";
            return;
        }

        await Shell.Current.GoToAsync("//counter/counter-results-tab");
    }

    [RelayCommand]
    private void Reset()
    {
        SelectedArmyType = ArmyTypeOptions[0];
        SelectedTargetRole = TargetRoleOptions[0];

        LandInfantry = 0;
        LandArtillery = 0;
        LandTank = 0;
        LandFighter = 0;
        LandBomber = 0;
        LandAntiAir = 0;
        LandCruiser = 0;
        LandBattleship = 0;

        NavalTransport = 0;
        NavalSubmarine = 0;
        NavalDestroyer = 0;
        NavalCruiser = 0;
        NavalBattleship = 0;
        NavalCarrier = 0;
        NavalFighter = 0;
        NavalBomber = 0;

        SimulationCount = 1000;
        _budgetOverrideIsCustom = false;
        BudgetOverrideText = TargetCost.ToString();
        CounterSearchProgress = 0;
        StatusMessage = "All counter search inputs reset to defaults.";
        ResetResults();
        RefreshDerivedState();
    }

    [RelayCommand]
    private void AnalyzeActualOutcome()
    {
        if (_lastMatchupStats is null)
        {
            ActualOutcomeStatusMessage = "Run a counter search first.";
            return;
        }

        string? validationError;
        Army actualAttackerArmy;
        Army actualDefenderArmy;

        if (IsLandArmy)
        {
            if (
                _lastMatchupStats.AttackingArmy is not LandArmy attackingArmy
                || _lastMatchupStats.DefendingArmy is not LandArmy defendingArmy
            )
            {
                ActualOutcomeStatusMessage = "The last counter matchup was not a land battle.";
                return;
            }

            validationError = ActualOutcomeAnalysisHelper.ValidateLandOutcome(
                ActualAttackerRemaining,
                ActualDefenderRemaining,
                attackingArmy.Units,
                defendingArmy.Units
            );
            actualAttackerArmy = ActualOutcomeAnalysisHelper.CreateLandArmy(
                ActualAttackerRemaining,
                true
            );
            actualDefenderArmy = ActualOutcomeAnalysisHelper.CreateLandArmy(
                ActualDefenderRemaining,
                false
            );
        }
        else
        {
            if (
                _lastMatchupStats.AttackingArmy is not NavalArmada attackingArmada
                || _lastMatchupStats.DefendingArmy is not NavalArmada defendingArmada
            )
            {
                ActualOutcomeStatusMessage = "The last counter matchup was not a naval battle.";
                return;
            }

            validationError = ActualOutcomeAnalysisHelper.ValidateNavalOutcome(
                ActualAttackerRemaining,
                ActualDefenderRemaining,
                attackingArmada.Units,
                defendingArmada.Units
            );
            actualAttackerArmy = ActualOutcomeAnalysisHelper.CreateNavalArmada(
                ActualAttackerRemaining,
                true
            );
            actualDefenderArmy = ActualOutcomeAnalysisHelper.CreateNavalArmada(
                ActualDefenderRemaining,
                false
            );
        }

        if (validationError is not null)
        {
            LuckyMetricRows = [];
            _highlightedActualResultValue = null;
            ActualOutcomeStatusMessage = validationError;
            OnPropertyChanged(nameof(HasLuckyStats));
            UpdateProbabilityChart();
            return;
        }

        try
        {
            var luckyStats = _lastMatchupStats.HowLuckyWasThisOutcome(
                actualAttackerArmy.Units,
                actualDefenderArmy.Units
            );

            _highlightedActualResultValue = actualAttackerArmy.Cost - actualDefenderArmy.Cost;
            LuckyMetricRows = ActualOutcomeAnalysisHelper.BuildLuckyMetricRows(luckyStats);
            ActualOutcomeStatusMessage = ActualOutcomeAnalysisHelper.FormatOutcomeStatus(
                actualAttackerArmy.Cost,
                actualDefenderArmy.Cost
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

    partial void OnSelectedArmyTypeChanged(string value)
    {
        _ = value;
        RefreshDerivedState();
        RefreshBudgetOverrideDefault();
    }

    partial void OnSelectedTargetRoleChanged(string value)
    {
        _ = value;
        RefreshDerivedState();
        RefreshBudgetOverrideDefault();
    }

    partial void OnBudgetOverrideTextChanged(string value)
    {
        _budgetOverrideIsCustom =
            !string.IsNullOrWhiteSpace(value)
            && !string.Equals(value.Trim(), TargetCost.ToString(), StringComparison.Ordinal);
        OnPropertyChanged(nameof(BudgetOverrideSummaryText));
    }

    partial void OnCounterSearchProgressChanged(double value)
    {
        _ = value;
        OnPropertyChanged(nameof(CounterSearchProgressFraction));
        OnPropertyChanged(nameof(CounterSearchProgressText));
    }

    private void AdjustUnit(string key, int delta)
    {
        switch (key)
        {
            case "land_infantry":
                LandInfantry = Clamp(LandInfantry + delta);
                break;
            case "land_artillery":
                LandArtillery = Clamp(LandArtillery + delta);
                break;
            case "land_tank":
                LandTank = Clamp(LandTank + delta);
                break;
            case "land_fighter":
                LandFighter = Clamp(LandFighter + delta);
                break;
            case "land_bomber":
                LandBomber = Clamp(LandBomber + delta);
                break;
            case "land_antiair":
                LandAntiAir = Clamp(LandAntiAir + delta);
                break;
            case "land_cruiser":
                LandCruiser = Clamp(LandCruiser + delta);
                break;
            case "land_battleship":
                LandBattleship = Clamp(LandBattleship + delta);
                break;

            case "naval_transport":
                NavalTransport = Clamp(NavalTransport + delta);
                break;
            case "naval_submarine":
                NavalSubmarine = Clamp(NavalSubmarine + delta);
                break;
            case "naval_destroyer":
                NavalDestroyer = Clamp(NavalDestroyer + delta);
                break;
            case "naval_cruiser":
                NavalCruiser = Clamp(NavalCruiser + delta);
                break;
            case "naval_battleship":
                NavalBattleship = Clamp(NavalBattleship + delta);
                break;
            case "naval_carrier":
                NavalCarrier = Clamp(NavalCarrier + delta);
                break;
            case "naval_fighter":
                NavalFighter = Clamp(NavalFighter + delta);
                break;
            case "naval_bomber":
                NavalBomber = Clamp(NavalBomber + delta);
                break;
        }

        RefreshDerivedState();
        RefreshBudgetOverrideDefault();
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

    private void AdjustBudgetOverride(int delta)
    {
        var currentBudget = TargetCost;

        if (int.TryParse(BudgetOverrideText.Trim(), out var parsedBudget))
        {
            currentBudget = parsedBudget;
        }

        currentBudget = Math.Max(1, currentBudget + delta);
        _budgetOverrideIsCustom = true;
        BudgetOverrideText = currentBudget.ToString();
        OnPropertyChanged(nameof(BudgetOverridePlaceholder));
    }

    private void RefreshDerivedState()
    {
        OnPropertyChanged(nameof(IsLandArmy));
        OnPropertyChanged(nameof(IsNavalArmy));
        OnPropertyChanged(nameof(IsTargetAttacking));
        OnPropertyChanged(nameof(IsTargetDefending));
        OnPropertyChanged(nameof(TargetArmyTypeLabel));
        OnPropertyChanged(nameof(TargetArmyRoleLabel));
        OnPropertyChanged(nameof(TargetCompositionTitle));
        OnPropertyChanged(nameof(TargetArmyHeader));
        OnPropertyChanged(nameof(TargetCost));
        OnPropertyChanged(nameof(BudgetOverridePlaceholder));
        OnPropertyChanged(nameof(BudgetOverrideSummaryText));
    }

    private void RefreshBudgetOverrideDefault()
    {
        if (_budgetOverrideIsCustom)
        {
            OnPropertyChanged(nameof(BudgetOverridePlaceholder));
            return;
        }

        BudgetOverrideText = TargetCost.ToString();
        OnPropertyChanged(nameof(BudgetOverridePlaceholder));
    }

    private void ResetResults()
    {
        _lastBestCounterArmy = null;
        TargetArmySummaryText = string.Empty;
        TargetArmyCompositionText = string.Empty;
        TargetArmyCostText = string.Empty;
        BestCounterArmySummaryText = string.Empty;
        BestCounterArmyCompositionText = string.Empty;
        BestCounterArmyCostText = string.Empty;
        ResultsSummaryText = string.Empty;
        BattleOutcomeRows = [];
        ArmyCompositionRows = [];
        RemainingUnitsRows = [];
        SummaryMetricRows = [];
        _lastMatchupStats = null;
        ResetActualOutcomeAnalysisState();
        UpdateProbabilityChart();
        OnPropertyChanged(nameof(HasResults));
        OnPropertyChanged(nameof(BestCounterCost));
    }

    private void PopulateResults(
        Army targetArmy,
        Army bestCounterArmy,
        SimulationStats matchupStats,
        int budgetUsed
    )
    {
        _lastBestCounterArmy = bestCounterArmy;

        TargetArmySummaryText =
            $"Target {GetArmyTypeLabel(targetArmy)} ({GetArmyRoleLabel(targetArmy)})";
        TargetArmyCompositionText = targetArmy.Units.ToString();
        TargetArmyCostText = $"{targetArmy.Cost} IPC";
        BestCounterArmySummaryText =
            $"Best Counter {GetArmyTypeLabel(bestCounterArmy)} ({GetArmyRoleLabel(bestCounterArmy)})";
        BestCounterArmyCompositionText = bestCounterArmy.Units.ToString();
        BestCounterArmyCostText = $"{bestCounterArmy.Cost} IPC";
        ResultsSummaryText =
            $"Budget used: {budgetUsed} IPC • Search simulations: {SimulationCount} • Final matchup simulations: {matchupStats.TotalBattles}";

        BattleOutcomeRows =
        [
            new OutcomeRow(
                "Attacker Wins",
                matchupStats.AttackerWon,
                $"{matchupStats.AttackerWonPercentage:F2}%"
            ),
            new OutcomeRow(
                "Defender Wins",
                matchupStats.DefenderWon,
                $"{matchupStats.DefenderWonPercentage:F2}%"
            ),
            new OutcomeRow("Draws", matchupStats.Draw, $"{matchupStats.DrawPercentage:F2}%"),
        ];

        var targetUnits = targetArmy.Units;
        var counterUnits = bestCounterArmy.Units;
        ArmyCompositionRows = IsLandArmy
            ?
            [
                new ComparisonRow(
                    "Infantry",
                    targetUnits.InfantryUnits.Count.ToString(),
                    counterUnits.InfantryUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Artillery",
                    targetUnits.ArtilleryUnits.Count.ToString(),
                    counterUnits.ArtilleryUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Tank",
                    targetUnits.TankUnits.Count.ToString(),
                    counterUnits.TankUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Fighter",
                    targetUnits.FighterUnits.Count.ToString(),
                    counterUnits.FighterUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Bomber",
                    targetUnits.BomberUnits.Count.ToString(),
                    counterUnits.BomberUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Anti-Air",
                    targetUnits.AntiAirUnits.Count.ToString(),
                    counterUnits.AntiAirUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Cruiser",
                    targetUnits.CruiserUnits.Count.ToString(),
                    counterUnits.CruiserUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Battleship",
                    targetUnits.BattleshipUnits.Count.ToString(),
                    counterUnits.BattleshipUnits.Count.ToString()
                ),
            ]
            :
            [
                new ComparisonRow(
                    "Transport",
                    targetUnits.TransportUnits.Count.ToString(),
                    counterUnits.TransportUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Submarine",
                    targetUnits.SubmarineUnits.Count.ToString(),
                    counterUnits.SubmarineUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Destroyer",
                    targetUnits.DestroyerUnits.Count.ToString(),
                    counterUnits.DestroyerUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Cruiser",
                    targetUnits.CruiserUnits.Count.ToString(),
                    counterUnits.CruiserUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Battleship",
                    targetUnits.BattleshipUnits.Count.ToString(),
                    counterUnits.BattleshipUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Carrier",
                    targetUnits.AircraftCarrierUnits.Count.ToString(),
                    counterUnits.AircraftCarrierUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Fighter",
                    targetUnits.FighterUnits.Count.ToString(),
                    counterUnits.FighterUnits.Count.ToString()
                ),
                new ComparisonRow(
                    "Bomber",
                    targetUnits.BomberUnits.Count.ToString(),
                    counterUnits.BomberUnits.Count.ToString()
                ),
            ];

        RemainingUnitsRows = IsLandArmy
            ?
            [
                new ComparisonRow(
                    "Infantry",
                    $"{matchupStats.AttackerRemainingUnitsAvg.InfantryUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.InfantryUnits:F2}"
                ),
                new ComparisonRow(
                    "Artillery",
                    $"{matchupStats.AttackerRemainingUnitsAvg.ArtilleryUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.ArtilleryUnits:F2}"
                ),
                new ComparisonRow(
                    "Tank",
                    $"{matchupStats.AttackerRemainingUnitsAvg.TankUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.TankUnits:F2}"
                ),
                new ComparisonRow(
                    "Fighter",
                    $"{matchupStats.AttackerRemainingUnitsAvg.FighterUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.FighterUnits:F2}"
                ),
                new ComparisonRow(
                    "Bomber",
                    $"{matchupStats.AttackerRemainingUnitsAvg.BomberUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.BomberUnits:F2}"
                ),
                new ComparisonRow(
                    "Anti-Air",
                    $"{matchupStats.AttackerRemainingUnitsAvg.AntiAirUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.AntiAirUnits:F2}"
                ),
                new ComparisonRow(
                    "Cruiser",
                    $"{matchupStats.AttackerRemainingUnitsAvg.CruiserUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.CruiserUnits:F2}"
                ),
                new ComparisonRow(
                    "Battleship",
                    $"{matchupStats.AttackerRemainingUnitsAvg.BattleshipUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.BattleshipUnits:F2}"
                ),
            ]
            :
            [
                new ComparisonRow(
                    "Transport",
                    $"{matchupStats.AttackerRemainingUnitsAvg.TransportUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.TransportUnits:F2}"
                ),
                new ComparisonRow(
                    "Submarine",
                    $"{matchupStats.AttackerRemainingUnitsAvg.SubmarineUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.SubmarineUnits:F2}"
                ),
                new ComparisonRow(
                    "Destroyer",
                    $"{matchupStats.AttackerRemainingUnitsAvg.DestroyerUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.DestroyerUnits:F2}"
                ),
                new ComparisonRow(
                    "Cruiser",
                    $"{matchupStats.AttackerRemainingUnitsAvg.CruiserUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.CruiserUnits:F2}"
                ),
                new ComparisonRow(
                    "Battleship",
                    $"{matchupStats.AttackerRemainingUnitsAvg.BattleshipUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.BattleshipUnits:F2}"
                ),
                new ComparisonRow(
                    "Carrier",
                    $"{matchupStats.AttackerRemainingUnitsAvg.AircraftCarrierUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.AircraftCarrierUnits:F2}"
                ),
                new ComparisonRow(
                    "Fighter",
                    $"{matchupStats.AttackerRemainingUnitsAvg.FighterUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.FighterUnits:F2}"
                ),
                new ComparisonRow(
                    "Bomber",
                    $"{matchupStats.AttackerRemainingUnitsAvg.BomberUnits:F2}",
                    $"{matchupStats.DefenderRemainingUnitsAvg.BomberUnits:F2}"
                ),
            ];

        var budgetDelta = bestCounterArmy.Cost - targetArmy.Cost;
        var budgetDeltaText = budgetDelta switch
        {
            > 0 => $"+{budgetDelta}",
            < 0 => budgetDelta.ToString(),
            _ => "0",
        };

        SummaryMetricRows =
        [
            new MetricRow("Target Army Type", TargetArmyTypeLabel),
            new MetricRow("Target Army Role", TargetArmyRoleLabel),
            new MetricRow("Target Cost", $"{targetArmy.Cost} IPC"),
            new MetricRow("Best Counter Cost", $"{bestCounterArmy.Cost} IPC"),
            new MetricRow("Budget", $"{budgetUsed} IPC ({budgetDeltaText} vs target)"),
            new MetricRow("Search Simulations", SimulationCount.ToString()),
            new MetricRow("Total Battles", matchupStats.TotalBattles.ToString()),
            new MetricRow("Average Attacker IPC Loss", $"{matchupStats.AttackerAvgCpLoss:F2}"),
            new MetricRow("Average Defender IPC Loss", $"{matchupStats.DefenderAvgCpLoss:F2}"),
        ];

        _lastMatchupStats = matchupStats;
        UpdateProbabilityChart();

        OnPropertyChanged(nameof(HasResults));
        OnPropertyChanged(nameof(BestCounterCost));
    }

    private void UpdateProbabilityChart()
    {
        var chartData = ProbabilityDistributionChartBuilder.Build(
            _lastMatchupStats,
            _highlightedActualResultValue
        );
        ProbabilityDistributionSeries = chartData.Series;
        ProbabilityDistributionXAxes = chartData.XAxes;
        ProbabilityDistributionYAxes = chartData.YAxes;
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

    private bool TryGetBudgetOverride(out int budget, out string errorMessage)
    {
        var budgetText = BudgetOverrideText.Trim();
        if (string.IsNullOrWhiteSpace(budgetText))
        {
            budget = TargetCost;
            errorMessage = string.Empty;
            return true;
        }

        if (!int.TryParse(budgetText, out budget) || budget <= 0)
        {
            errorMessage = "Budget override must be a positive whole number.";
            return false;
        }

        errorMessage = string.Empty;
        return true;
    }

    private ArmyBuilder CreateBuilder() =>
        IsLandArmy ? new LandArmyBuilder() : new NavalArmadaBuilder();

    private Army CreateTargetArmy()
    {
        if (IsLandArmy)
        {
            return new LandArmy(
                isAttacking: IsTargetAttacking,
                infantryCount: LandInfantry,
                artilleryCount: LandArtillery,
                tankCount: LandTank,
                fighterCount: LandFighter,
                bomberCount: LandBomber,
                antiAirCount: LandAntiAir,
                cruiserCount: LandCruiser,
                battleshipCount: LandBattleship
            );
        }

        return new NavalArmada(
            isAttacking: IsTargetAttacking,
            transportCount: NavalTransport,
            submarineCount: NavalSubmarine,
            destroyerCount: NavalDestroyer,
            cruiserCount: NavalCruiser,
            battleshipCount: NavalBattleship,
            carrierCount: NavalCarrier,
            fighterCount: NavalFighter,
            bomberCount: NavalBomber
        );
    }

    private int GetLandTargetCost() =>
        new LandArmy(
            isAttacking: IsTargetAttacking,
            infantryCount: LandInfantry,
            artilleryCount: LandArtillery,
            tankCount: LandTank,
            fighterCount: LandFighter,
            bomberCount: LandBomber,
            antiAirCount: LandAntiAir,
            cruiserCount: LandCruiser,
            battleshipCount: LandBattleship
        ).Cost;

    private int GetNavalTargetCost() =>
        new NavalArmada(
            isAttacking: IsTargetAttacking,
            transportCount: NavalTransport,
            submarineCount: NavalSubmarine,
            destroyerCount: NavalDestroyer,
            cruiserCount: NavalCruiser,
            battleshipCount: NavalBattleship,
            carrierCount: NavalCarrier,
            fighterCount: NavalFighter,
            bomberCount: NavalBomber
        ).Cost;

    private static string GetArmyTypeLabel(Army army) =>
        army is LandArmy ? "Land Army" : "Naval Armada";

    private static string GetArmyRoleLabel(Army army) =>
        army.IsAttacking ? "Attacking" : "Defending";

    private static int Clamp(int value) => Math.Max(0, value);
}
