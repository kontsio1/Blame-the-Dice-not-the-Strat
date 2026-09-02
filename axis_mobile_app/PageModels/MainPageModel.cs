using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;

namespace axis_mobile_app.PageModels;

public partial class MainPageModel : ObservableObject
{
    [ObservableProperty] private int _attackerInfantry = 3;
    [ObservableProperty] private int _attackerArtillery = 1;
    [ObservableProperty] private int _attackerTank = 1;
    [ObservableProperty] private int _attackerFighter;
    [ObservableProperty] private int _attackerBomber;
    [ObservableProperty] private int _attackerAntiAir;
    [ObservableProperty] private int _attackerCruiser;
    [ObservableProperty] private int _attackerBattleship;

    [ObservableProperty] private int _defenderInfantry = 4;
    [ObservableProperty] private int _defenderArtillery = 1;
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

            ResultsText = BuildResultsText(stats);
            StatusMessage = "Simulation complete.";
            OnPropertyChanged(nameof(AttackerCost));
            OnPropertyChanged(nameof(DefenderCost));
        }
        catch (Exception ex)
        {
            StatusMessage = "Simulation failed.";
            ResultsText = ex.Message;
        }
        finally
        {
            IsBusy = false;
        }
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

    private static string BuildResultsText(SimulationStats stats)
    {
        var sb = new StringBuilder();
        sb.AppendLine("--- Simulation Summary ---");
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