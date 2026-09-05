using axis_console_project.Resolvers;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Resolvers;

public class ArmyBuilderTests
{
    #region CreateArmiesFromCost - Land Army Tests

    [Fact]
    public void CreateArmiesFromCost_WithLandArmyBuilder_ReturnsLandArmies()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 30;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().AllBeOfType<LandArmy>();
    }

    [Fact]
    public void CreateArmiesFromCost_LandArmies_AllWithinMaxCost()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeLessThanOrEqualTo(maxCost));
    }

    [Fact]
    public void CreateArmiesFromCost_LandArmies_AllHaveNonZeroCost()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeGreaterThan(0));
    }

    [Fact]
    public void CreateArmiesFromCost_LandArmies_SetsAttackingFlag()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 30;

        // Act
        var armiesAttacking = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();
        var armiesDefending = builder.CreateArmiesFromCost(maxCost, isAttacking: false).ToList();

        // Assert
        armiesAttacking.Should().AllSatisfy(army => army.IsAttacking.Should().BeTrue());
        armiesDefending.Should().AllSatisfy(army => army.IsAttacking.Should().BeFalse());
    }

    #endregion

    #region CreateArmiesFromCost - Naval Army Tests

    [Fact]
    public void CreateArmiesFromCost_WithNavalArmadaBuilder_ReturnsNavalArmadas()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().AllBeOfType<NavalArmada>();
    }

    [Fact]
    public void CreateArmiesFromCost_NavalArmies_AllWithinMaxCost()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 60;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeLessThanOrEqualTo(maxCost));
    }

    [Fact]
    public void CreateArmiesFromCost_NavalArmies_AllHaveNonZeroCost()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 60;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();

        // Assert
        armies.Should().AllSatisfy(army => army.Cost.Should().BeGreaterThan(0));
    }

    [Fact]
    public void CreateArmiesFromCost_NavalArmies_SetsAttackingFlag()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int maxCost = 50;

        // Act
        var armiesAttacking = builder.CreateArmiesFromCost(maxCost, isAttacking: true).ToList();
        var armiesDefending = builder.CreateArmiesFromCost(maxCost, isAttacking: false).ToList();

        // Assert
        armiesAttacking.Should().AllSatisfy(army => army.IsAttacking.Should().BeTrue());
        armiesDefending.Should().AllSatisfy(army => army.IsAttacking.Should().BeFalse());
    }

    #endregion

    #region CreateCounterArmy Tests

    [Fact]
    public void CreateCounterArmy_WithLandArmy_ReturnsLandArmy()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        var targetArmy = new LandArmy(isAttacking: true, infantryCount: 5);

        // Act
        var counterArmy = builder.CreateCounterArmy(targetArmy, sims: 10, verbose: false);

        // Assert
        counterArmy.Should().BeOfType<LandArmy>();
        counterArmy.Cost.Should().BeLessThanOrEqualTo(targetArmy.Cost);
    }

    // [Fact]
    // public void CreateCounterArmy_WithNavalArmy_ReturnsNavalArmada()
    // {
    //     // Arrange
    //     var builder = new NavalArmadaBuilder();
    //     var targetArmy = new NavalArmada(isAttacking: true, transportCount: 5);
    //
    //     // Act
    //     var counterArmy = builder.CreateCounterArmy(targetArmy, sims: 10, verbose: false);
    //
    //     // Assert
    //     counterArmy.Should().BeOfType<NavalArmada>();
    //     counterArmy.Cost.Should().BeLessThanOrEqualTo(targetArmy.Cost);
    // }

    [Fact]
    public void CreateCounterArmy_WithCustomCost_RespectsMaxCost()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        var targetArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        int customCost = 100;

        // Act
        var counterArmy = builder.CreateCounterArmy(targetArmy, cost: customCost, sims: 10, verbose: false);

        // Assert
        counterArmy.Cost.Should().BeLessThanOrEqualTo(customCost);
    }

    [Fact]
    public void CreateCounterArmy_WhenNoValidArmyExists_ThrowsException()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        var targetArmy = new LandArmy(isAttacking: true, infantryCount: 1);

        // Act & Assert
        var action = () => builder.CreateCounterArmy(targetArmy, cost: 1, sims: 10, verbose: false);
        action.Should().Throw<Exception>().WithMessage("*Couldn't create army*");
    }

    #endregion

    #region Land Army Composition Tests

    [Fact]
    public void GetAllLandCombinations_ReturnsMultipleCombinations()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int cost = 30;

        // Act
        var armies = builder.CreateArmiesFromCost(cost, true).ToList();

        // Assert
        armies.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public void GetAllLandCombinations_CostsAreCloseToTarget()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int cost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(cost, true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().AllSatisfy(army =>
        {
            army.Cost.Should().BeLessThanOrEqualTo(cost);
            army.Cost.Should().BeGreaterThanOrEqualTo(cost - 3);
        });
    }

    #endregion

    #region Naval Army Composition Tests

    [Fact]
    public void GetAllNavalCombinations_ReturnsMultipleCombinations()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int cost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(cost, true).ToList();

        // Assert
        armies.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public void GetAllNavalCombinations_CostsAreCloseToTarget()
    {
        // Arrange
        var builder = new NavalArmadaBuilder();
        int cost = 50;

        // Act
        var armies = builder.CreateArmiesFromCost(cost, true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().AllSatisfy(army =>
        {
            army.Cost.Should().BeLessThanOrEqualTo(cost);
            army.Cost.Should().BeGreaterThanOrEqualTo(cost - 6);
        });
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void CreateArmiesFromCost_WithVeryLowCost_ReturnsEmptyOrSingleUnits()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 3; // Infantry costs 3

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, true).ToList();

        // Assert
        armies.Should().NotBeEmpty();
        armies.Should().HaveCount(1);
        armies[0].Cost.Should().Be(3);
    }

    [Fact]
    public void CreateArmiesFromCost_WithExactUnitCost_CreatesArmy()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 3; // Infantry costs 3

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, true).ToList();

        // Assert
        armies.Should().HaveCount(1);
        armies[0].Cost.Should().Be(3);
    }

    [Fact]
    public void CreateArmiesFromCost_WithHighCost_CreatesMultipleNearTargetVariations()
    {
        // Arrange
        var builder = new LandArmyBuilder();
        int maxCost = 100;

        // Act
        var armies = builder.CreateArmiesFromCost(maxCost, true).ToList();

        // Assert
        armies.Count.Should().BeGreaterThan(10);
        armies.Should().AllSatisfy(army =>
        {
            army.Cost.Should().BeLessThanOrEqualTo(maxCost);
            army.Cost.Should().BeGreaterThanOrEqualTo(maxCost - 3);
        });
    }

    #endregion
}

