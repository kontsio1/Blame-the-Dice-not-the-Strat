using axis_console_project.Army;
using axis_console_project.Battle;
using axis_console_project.UnitTypes.Land;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Battle;

public class BattleOutcomeTests
{
    #region BattleOutcome Enum Tests
    
    [Fact]
    public void BattleOutcome_ShouldHaveAttackerVictoryValue()
    {
        // Assert
        BattleOutcome.AttackerVictory.Should().BeDefined();
    }

    [Fact]
    public void BattleOutcome_ShouldHaveDefenderVictoryValue()
    {
        // Assert
        BattleOutcome.DefenderVictory.Should().BeDefined();
    }

    [Fact]
    public void BattleOutcome_ShouldHaveDrawValue()
    {
        // Assert
        BattleOutcome.Draw.Should().BeDefined();
    }

    [Fact]
    public void BattleOutcome_ShouldHaveExactlyThreeValues()
    {
        // Arrange
        var values = Enum.GetValues<BattleOutcome>();

        // Assert
        values.Should().HaveCount(3);
    }
    
    #endregion
}

public class BattleResultTests
{
    #region Constructor Tests
    
    [Fact]
    public void BattleResult_ShouldStoreAttackingArmy()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.AttackingArmy.Should().BeSameAs(attackingArmy);
    }

    [Fact]
    public void BattleResult_ShouldStoreDefendingArmy()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.DefendingArmy.Should().BeSameAs(defendingArmy);
    }

    [Fact]
    public void BattleResult_ShouldStoreBattleOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.DefenderVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.BattleOutcome.Should().Be(BattleOutcome.DefenderVictory);
    }

    [Fact]
    public void BattleResult_ShouldStoreAttackerRemainingUnits()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.AttackerRemainingUnits.Should().NotBeNull();
        result.AttackerRemainingUnits.InfantryUnits.Should().HaveCount(5);
    }

    [Fact]
    public void BattleResult_ShouldStoreDefenderRemainingUnits()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.DefenderRemainingUnits.Should().NotBeNull();
        result.DefenderRemainingUnits.InfantryUnits.Should().HaveCount(3);
    }
    
    #endregion

    #region Remaining Units Tests
    
    [Fact]
    public void BattleResult_AttackerRemainingUnits_ShouldBeUnitsType()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2, tankCount: 1);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 1);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.AttackerRemainingUnits.Should().BeOfType<Units>();
    }

    [Fact]
    public void BattleResult_DefenderRemainingUnits_ShouldBeUnitsType()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3, tankCount: 1);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.DefenderVictory,
            remainingAttackerUnits,
            remainingDefenderUnits);

        // Assert
        result.DefenderRemainingUnits.Should().BeOfType<Units>();
    }

    [Fact]
    public void BattleResult_WithEmptyRemainingUnits_ShouldHandleEmptyList()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var emptyList = new List<Unit>();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.Draw,
            emptyList,
            emptyList);

        // Assert
        result.AttackerRemainingUnits.GetAllUnits().Should().BeEmpty();
        result.DefenderRemainingUnits.GetAllUnits().Should().BeEmpty();
    }

    [Fact]
    public void BattleResult_RemainingUnits_ShouldPreserveUnitTypes()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2, artilleryCount: 1, tankCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 1);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttackerUnits,
            new List<Unit>());

        // Assert
        result.AttackerRemainingUnits.InfantryUnits.Should().HaveCount(2);
        result.AttackerRemainingUnits.ArtilleryUnits.Should().HaveCount(1);
        result.AttackerRemainingUnits.TankUnits.Should().HaveCount(3);
    }
    
    #endregion

    #region All Outcome Types Tests
    
    [Fact]
    public void BattleResult_WithAttackerVictory_ShouldStoreCorrectOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            attackingArmy.GetAllAliveUnits(),
            new List<Unit>());

        // Assert
        result.BattleOutcome.Should().Be(BattleOutcome.AttackerVictory);
    }

    [Fact]
    public void BattleResult_WithDefenderVictory_ShouldStoreCorrectOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5);

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.DefenderVictory,
            new List<Unit>(),
            defendingArmy.GetAllAliveUnits());

        // Assert
        result.BattleOutcome.Should().Be(BattleOutcome.DefenderVictory);
    }

    [Fact]
    public void BattleResult_WithDraw_ShouldStoreCorrectOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);

        // Act
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.Draw,
            new List<Unit>(),
            new List<Unit>());

        // Assert
        result.BattleOutcome.Should().Be(BattleOutcome.Draw);
    }
    
    #endregion
}

