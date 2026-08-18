using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Simulation;

public class SimulationTests
{
    #region Constructor Tests
    
    [Fact]
    public void Simulation_Constructor_ShouldCreateInstance()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);

        // Act
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Assert
        simulation.Should().NotBeNull();
    }
    
    #endregion

    #region Run Method Tests
    
    [Fact]
    public void Run_ShouldReturnSimulationStats()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(10);
        var stats = simulation.Stats;

        // Assert
        stats.Should().NotBeNull();
        stats.Should().BeOfType<SimulationStats>();
    }

    [Fact]
    public void Run_ShouldRunSpecifiedNumberOfSimulations()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        const int numSimulations = 100;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(numSimulations);
    }

    [Fact]
    public void Run_TotalBattles_ShouldEqualSumOfOutcomes()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        const int numSimulations = 50;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        (stats.AttackerWon + stats.DefenderWon + stats.Draw).Should().Be(numSimulations);
    }

    [Fact]
    public void Run_WithSingleSimulation_ShouldComplete()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(1);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(1);
    }

    [Fact]
    public void Run_WithZeroSimulations_ShouldReturnEmptyStats()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(0);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(0);
        stats.AttackerWon.Should().Be(0);
        stats.DefenderWon.Should().Be(0);
        stats.Draw.Should().Be(0);
    }

    [Fact]
    public void Run_ShouldSetArmiesInStats()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(10);
        var stats = simulation.Stats;

        // Assert
        stats.AttackingArmy.Should().BeSameAs(attackingArmy);
        stats.DefendingArmy.Should().BeSameAs(defendingArmy);
    }
    
    #endregion

    #region Statistical Outcome Tests
    
    [Fact]
    public void Run_WithOverwhelmingAttacker_ShouldHaveHighAttackerWinRate()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 20, tankCount: 10);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        const int numSimulations = 100;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        stats.AttackerWonPercentage.Should().BeGreaterThan(80);
    }

    [Fact]
    public void Run_WithOverwhelmingDefender_ShouldHaveHighDefenderWinRate()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 20, tankCount: 10);
        const int numSimulations = 100;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        stats.DefenderWonPercentage.Should().BeGreaterThan(80);
    }

    [Fact]
    public void Run_WithEqualForces_ShouldProduceVariedOutcomes()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5, tankCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5, tankCount: 3);
        const int numSimulations = 100;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert - Both sides should win some battles
        stats.AttackerWon.Should().BeGreaterThan(0);
        stats.DefenderWon.Should().BeGreaterThan(0);
    }
    
    #endregion

    #region Naval Battle Simulation Tests
    
    [Fact]
    public void Run_NavalBattle_ShouldComplete()
    {
        // Arrange
        var attackingFleet = new NavalArmada(isAttacking: true, destroyerCount: 3, submarineCount: 2);
        var defendingFleet = new NavalArmada(isAttacking: false, destroyerCount: 2, cruiserCount: 1);
        var simulation = new Simulations.Simulation(attackingFleet, defendingFleet);

        // Act
        simulation.Run(50);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(50);
    }

    [Fact]
    public void Run_NavalBattle_ShouldTrackCorrectStatistics()
    {
        // Arrange
        var attackingFleet = new NavalArmada(isAttacking: true, destroyerCount: 5, battleshipCount: 2);
        var defendingFleet = new NavalArmada(isAttacking: false, submarineCount: 2);
        const int numSimulations = 50;
        var simulation = new Simulations.Simulation(attackingFleet, defendingFleet);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(numSimulations);
        (stats.AttackerWon + stats.DefenderWon + stats.Draw).Should().Be(numSimulations);
    }
    
    #endregion

    #region Mixed Forces Simulation Tests
    
    [Fact]
    public void Run_WithMixedLandForces_ShouldComplete()
    {
        // Arrange
        var attackingArmy = new LandArmy(
            isAttacking: true,
            infantryCount: 5,
            artilleryCount: 2,
            tankCount: 3,
            fighterCount: 2,
            bomberCount: 1);
        var defendingArmy = new LandArmy(
            isAttacking: false,
            infantryCount: 7,
            artilleryCount: 1,
            tankCount: 2,
            fighterCount: 1,
            antiAirCount: 1);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(50);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(50);
    }

    [Fact]
    public void Run_WithNavalBombardment_ShouldComplete()
    {
        // Arrange
        var attackingArmy = new LandArmy(
            isAttacking: true,
            infantryCount: 5,
            cruiserCount: 2,
            battleshipCount: 1);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(30);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(30);
    }
    
    #endregion

    #region ToString Tests
    
    [Fact]
    public void ToString_ShouldContainArmyInformation()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        var result = simulation.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("Simulation");
        result.Should().Contain("vs");
    }

    [Fact]
    public void ToString_ShouldContainArmyDetails()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        var result = simulation.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
    }
    
    #endregion

    #region Large Scale Simulation Tests
    
    [Fact]
    public void Run_LargeNumberOfSimulations_ShouldComplete()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5, tankCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 4, tankCount: 2);
        const int numSimulations = 500;
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(numSimulations);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(numSimulations);
    }

    [Fact]
    public void Run_LargeArmies_ShouldComplete()
    {
        // Arrange
        var attackingArmy = new LandArmy(
            isAttacking: true,
            infantryCount: 20,
            artilleryCount: 10,
            tankCount: 15,
            fighterCount: 5,
            bomberCount: 3);
        var defendingArmy = new LandArmy(
            isAttacking: false,
            infantryCount: 25,
            artilleryCount: 8,
            tankCount: 10,
            fighterCount: 4,
            bomberCount: 2,
            antiAirCount: 2);
        var simulation = new Simulations.Simulation(attackingArmy, defendingArmy);

        // Act
        simulation.Run(20);
        var stats = simulation.Stats;

        // Assert
        stats.TotalBattles.Should().Be(20);
    }
    
    #endregion
}
