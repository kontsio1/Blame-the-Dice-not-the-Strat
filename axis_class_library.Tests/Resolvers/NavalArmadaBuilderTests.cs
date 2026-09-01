using axis_console_project.Armies;
using axis_console_project.Resolvers;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Sea;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Resolvers;

public class NavalArmadaBuilderTests
{
    #region CreateArmiesFromCost - Return Type

    [Fact]
    public void CreateArmiesFromCost_ReturnsNavalArmadas()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();

        // Act
        var armies = builder.CreateArmiesFromCost(30, isAttacking: true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().AllBeOfType<NavalArmada>();
    }

    [Fact]
    public void CreateArmiesFromCost_SetsAttackingFlag_WhenTrue()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();

        // Act
        var armies = builder.CreateArmiesFromCost(30, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.IsAttacking.Should().BeTrue());
    }

    [Fact]
    public void CreateArmiesFromCost_SetsAttackingFlag_WhenFalse()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();

        // Act
        var armies = builder.CreateArmiesFromCost(30, isAttacking: false).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.IsAttacking.Should().BeFalse());
    }  
    #endregion

    #region CreateArmiesFromCost - Cost Constraints

    [Fact]
    public void CreateArmiesFromCost_AllArmiesWithinMaxCost()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeLessThanOrEqualTo(maxCost));
    }

    [Fact]
    public void CreateArmiesFromCost_AllArmiesHaveNonZeroCost()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();

        // Act
        var armies = builder.CreateArmiesFromCost(30, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeGreaterThan(0));
    }

    [Fact]
    public void CreateArmiesFromCost_CostIsWithinCheapestUnitRangeOfBudget()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 30;
        int cheapestNavalUnitCost = Submarine.UnitCost; // Submarine is the cheapest naval unit

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army =>
            army.Cost.Should().BeGreaterThanOrEqualTo(maxCost - cheapestNavalUnitCost));
    }

    [Fact]
    public void CreateArmiesFromCost_WithExactUnitCost_ReturnsSingleEntry()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = Submarine.UnitCost; // 6 - only one submarine fits

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().HaveCount(1);
        armies[0].Cost.Should().Be(Submarine.UnitCost);
    }

    [Fact]
    public void CreateArmiesFromCost_WithHighCost_ReturnsMultipleCombinations()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();

        // Act
        var armies = builder.CreateArmiesFromCost(50, isAttacking: true).ToList();

        // Assert
        armies.Count.Should().BeGreaterThan(1);
    }

    #endregion

    #region CreateCounterArmy - Default Behaviour

    [Fact]
    public void CreateCounterArmy_AgainstAttackingNavalArmy_ReturnsDefendingNavalArmada()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(targetArmy, sims: 10, verbose: false);

        // Assert
        counterArmy.Should().BeOfType<NavalArmada>();
        counterArmy.IsAttacking.Should().BeFalse();
    }

    [Fact]
    public void CreateCounterArmy_AgainstDefendingNavalArmy_ReturnsAttackingNavalArmada()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: false, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(targetArmy, sims: 10, verbose: false);

        // Assert
        counterArmy.Should().BeOfType<NavalArmada>();
        counterArmy.IsAttacking.Should().BeTrue();
    }

    [Fact]
    public void CreateCounterArmy_WithCustomCost_ReturnsArmyWithinThatCost()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 1);
        int customCost = 30;

        // Act
        var counterArmy = builder.CreateCounterArmy(targetArmy, cost: customCost, sims: 10, verbose: false);

        // Assert
        counterArmy.Cost.Should().BeLessThanOrEqualTo(customCost);
    }

    [Fact]
    public void CreateCounterArmy_WhenCostTooLow_ThrowsException()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 1);

        // Act & Assert
        var action = () => builder.CreateCounterArmy(targetArmy, cost: 1, sims: 10, verbose: false);
        action.Should().Throw<Exception>().WithMessage("*Couldn't create army*");
    }

    #endregion

    #region CreateCounterArmy - Selection Strategy

    [Fact]
    public void CreateCounterArmy_WithMaximiseSelection_ReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            selectionStrategy: Selection.Maximise,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Cost.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CreateCounterArmy_WithMinimiseSelection_ReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            selectionStrategy: Selection.Minimise,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Cost.Should().BeGreaterThan(0);
    }

    #endregion

    #region CreateCounterArmy - Custom Property Selector

    [Fact]
    public void CreateCounterArmy_WithCustomWinPercentageSelector_ReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            propertySelectorForCounter: stats => stats.DefenderWonPercentage,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Should().BeOfType<NavalArmada>();
    }

    [Fact]
    public void CreateCounterArmy_WithAvgCpLossSelector_ReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        // Minimise attacker's average CP loss to find most cost-efficient counter
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            propertySelectorForCounter: stats => stats.AttackerAvgCpLoss,
            selectionStrategy: Selection.Minimise,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Cost.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CreateCounterArmy_WithDrawPercentageSelector_ReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, submarineCount: 2);

        // Act
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            propertySelectorForCounter: stats => stats.DrawPercentage,
            selectionStrategy: Selection.Maximise,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Cost.Should().BeGreaterThan(0);
    }

    [Fact]
    public void CreateCounterArmy_WithDefenderCpLossSelector_MinimisedReturnsValidArmy()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        var targetArmy = new NavalArmada(isAttacking: true, destroyerCount: 1);

        // Act
        var counterArmy = builder.CreateCounterArmy(
            targetArmy,
            sims: 10,
            propertySelectorForCounter: stats => stats.DefenderAvgCpLoss,
            selectionStrategy: Selection.Minimise,
            verbose: false
        );

        // Assert
        counterArmy.Should().NotBeNull();
        counterArmy.Cost.Should().BeGreaterThan(0);
    }

    #endregion
}

