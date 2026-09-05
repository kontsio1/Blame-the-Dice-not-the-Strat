using axis_console_project.Armies;
using axis_console_project.Battles;
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
    public void BattleResult_ShouldStoreBattleOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var remainingAttackerUnits = attackingArmy.GetAllAliveUnits();
        var remainingDefenderUnits = defendingArmy.GetAllAliveUnits();

        // Act
        var result = new BattleResult(
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
            BattleOutcome.Draw,
            new List<Unit>(),
            new List<Unit>());

        // Assert
        result.BattleOutcome.Should().Be(BattleOutcome.Draw);
    }
    
    #endregion
}

public class IdenticalArmySelfBattleTests
{
    #region Identical Army Self-Battle Tests
    
    private readonly int _totalBattles = 50000;
    
    [Fact]
    public void IdenticalArmies_WithTanksOnly_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 5);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 5);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(30).And.BeLessThan(70);
    }

    [Fact]
    public void IdenticalArmies_WithArtilleryOnly_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, artilleryCount: 5);
            var defendingArmy = new LandArmy(isAttacking: false, artilleryCount: 5);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(45).And.BeLessThan(55);
    }

    [Fact]
    public void IdenticalArmies_WithMixedUnits_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 3, artilleryCount: 2);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 3, artilleryCount: 2);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(45).And.BeLessThan(55);
    }

    [Fact]
    public void IdenticalArmies_WithTanksAndArtillery_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 4, artilleryCount: 3);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 4, artilleryCount: 3);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(45).And.BeLessThan(55);
    }

    [Fact]
    public void IdenticalArmies_LargeScale_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 10, artilleryCount: 5);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 10, artilleryCount: 5);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(45).And.BeLessThan(55);
    }

    [Fact]
    public void IdenticalArmies_WithSmallTankForce_ShouldHaveApprox50PercentVictoryChance()
    {
        // Arrange
        
        int attackerWins = 0;
        int defenderWins = 0;
        int draws = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 2);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 2);
            
            var battle = new axis_console_project.Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
            else if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
            else
                draws++;
        }

        // Assert
        // Attacker should win approximately 50% of battles (±20% tolerance)
        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;
        attackerWinPercentage.Should().BeGreaterThan(40).And.BeLessThan(60);
    }

    [Fact]
    public void IdenticalArmies_ShouldNotHaveDominantOutcome()
    {
        // Arrange - Run multiple battles to ensure no one side dominates
        int _totalBattles = 1000;
        int attackerWins = 0;

        // Act
        for (int i = 0; i < _totalBattles; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, tankCount: 5);
            var defendingArmy = new LandArmy(isAttacking: false, tankCount: 5);
            
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
        }

        var attackerWinPercentage = (attackerWins * 100.0) / _totalBattles;

        // Assert
        // Neither side should consistently win more than 75% or win less than 25%
        attackerWinPercentage.Should().BeGreaterThan(40).And.BeLessThan(60);
    }
    
    #endregion
}
