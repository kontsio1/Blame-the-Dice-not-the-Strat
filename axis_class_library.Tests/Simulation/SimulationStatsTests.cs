using axis_console_project.Armies;
using axis_console_project.Battles;
using axis_console_project.Simulations;
using axis_console_project.UnitTypes.Land;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Simulation;

public class SimulationStatsTests
{
    #region Initial State Tests
    
    [Fact]
    public void SimulationStats_InitialState_AttackerWonShouldBeZero()
    {
        // Arrange & Act
        var stats = new SimulationStats();

        // Assert
        stats.AttackerWon.Should().Be(0);
    }

    [Fact]
    public void SimulationStats_InitialState_DefenderWonShouldBeZero()
    {
        // Arrange & Act
        var stats = new SimulationStats();

        // Assert
        stats.DefenderWon.Should().Be(0);
    }

    [Fact]
    public void SimulationStats_InitialState_DrawShouldBeZero()
    {
        // Arrange & Act
        var stats = new SimulationStats();

        // Assert
        stats.Draw.Should().Be(0);
    }

    [Fact]
    public void SimulationStats_InitialState_TotalBattlesShouldBeZero()
    {
        // Arrange & Act
        var stats = new SimulationStats();

        // Assert
        stats.TotalBattles.Should().Be(0);
    }
    
    #endregion

    #region TotalBattles Computation Tests
    
    [Fact]
    public void TotalBattles_ShouldEqualSumOfAllOutcomes()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 5,
            DefenderWon = 3,
            Draw = 2
        };

        // Assert
        stats.TotalBattles.Should().Be(10);
    }

    [Fact]
    public void TotalBattles_WithOnlyAttackerWins_ShouldBeCorrect()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 10,
            DefenderWon = 0,
            Draw = 0
        };

        // Assert
        stats.TotalBattles.Should().Be(10);
    }

    [Fact]
    public void TotalBattles_WithOnlyDefenderWins_ShouldBeCorrect()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 0,
            DefenderWon = 15,
            Draw = 0
        };

        // Assert
        stats.TotalBattles.Should().Be(15);
    }

    [Fact]
    public void TotalBattles_WithOnlyDraws_ShouldBeCorrect()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 0,
            DefenderWon = 0,
            Draw = 8
        };

        // Assert
        stats.TotalBattles.Should().Be(8);
    }
    
    #endregion

    #region Percentage Computation Tests
    
    [Fact]
    public void AttackerWonPercentage_ShouldCalculateCorrectly()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 50,
            DefenderWon = 30,
            Draw = 20
        };

        // Assert
        stats.AttackerWonPercentage.Should().Be(50.0);
    }

    [Fact]
    public void DefenderWonPercentage_ShouldCalculateCorrectly()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 50,
            DefenderWon = 30,
            Draw = 20
        };

        // Assert
        stats.DefenderWonPercentage.Should().Be(30.0);
    }

    [Fact]
    public void DrawPercentage_ShouldCalculateCorrectly()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 50,
            DefenderWon = 30,
            Draw = 20
        };

        // Assert
        stats.DrawPercentage.Should().Be(20.0);
    }

    [Fact]
    public void AllPercentages_ShouldSumTo100()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 45,
            DefenderWon = 35,
            Draw = 20
        };

        // Act
        var total = stats.AttackerWonPercentage + stats.DefenderWonPercentage + stats.DrawPercentage;

        // Assert
        total.Should().Be(100.0);
    }

    [Fact]
    public void Percentages_WithZeroTotalBattles_ShouldBeZero()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.AttackerWonPercentage.Should().Be(0);
        stats.DefenderWonPercentage.Should().Be(0);
        stats.DrawPercentage.Should().Be(0);
    }

    [Fact]
    public void AttackerWonPercentage_With100PercentWinRate_ShouldBe100()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 100,
            DefenderWon = 0,
            Draw = 0
        };

        // Assert
        stats.AttackerWonPercentage.Should().Be(100.0);
    }

    [Fact]
    public void DefenderWonPercentage_With100PercentWinRate_ShouldBe100()
    {
        // Arrange
        var stats = new SimulationStats
        {
            AttackerWon = 0,
            DefenderWon = 100,
            Draw = 0
        };

        // Assert
        stats.DefenderWonPercentage.Should().Be(100.0);
    }
    
    #endregion

    #region RecordResult Tests
    
    [Fact]
    public void RecordResult_AttackerVictory_ShouldIncrementAttackerWon()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            attackingArmy.GetAllAliveUnits(),
            new List<Unit>());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerWon.Should().Be(1);
        stats.DefenderWon.Should().Be(0);
        stats.Draw.Should().Be(0);
    }

    [Fact]
    public void RecordResult_DefenderVictory_ShouldIncrementDefenderWon()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5);
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.DefenderVictory,
            new List<Unit>(),
            defendingArmy.GetAllAliveUnits());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerWon.Should().Be(0);
        stats.DefenderWon.Should().Be(1);
        stats.Draw.Should().Be(0);
    }

    [Fact]
    public void RecordResult_Draw_ShouldIncrementDraw()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.Draw,
            new List<Unit>(),
            new List<Unit>());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerWon.Should().Be(0);
        stats.DefenderWon.Should().Be(0);
        stats.Draw.Should().Be(1);
    }

    [Fact]
    public void RecordResult_MultipleResults_ShouldAccumulateCorrectly()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);

        var attackerWin = new BattleResult(attackingArmy, defendingArmy, BattleOutcome.AttackerVictory, 
            attackingArmy.GetAllAliveUnits(), new List<Unit>());
        var defenderWin = new BattleResult(attackingArmy, defendingArmy, BattleOutcome.DefenderVictory, 
            new List<Unit>(), defendingArmy.GetAllAliveUnits());
        var draw = new BattleResult(attackingArmy, defendingArmy, BattleOutcome.Draw, 
            new List<Unit>(), new List<Unit>());

        // Act
        stats.RecordResult(attackerWin);
        stats.RecordResult(attackerWin);
        stats.RecordResult(defenderWin);
        stats.RecordResult(draw);

        // Assert
        stats.AttackerWon.Should().Be(2);
        stats.DefenderWon.Should().Be(1);
        stats.Draw.Should().Be(1);
        stats.TotalBattles.Should().Be(4);
    }
    
    #endregion

    #region CP Loss Tracking Tests
    
    [Fact]
    public void AttackerAvgCpLoss_WithNoResults_ShouldBeZero()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.AttackerAvgCpLoss.Should().Be(0);
    }

    [Fact]
    public void DefenderAvgCpLoss_WithNoResults_ShouldBeZero()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.DefenderAvgCpLoss.Should().Be(0);
    }

    [Fact]
    public void RecordResult_ShouldTrackCpLoss()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        
        // Simulate some casualties - attacker loses 2 infantry
        var attackerUnits = attackingArmy.GetAllAliveUnits();
        attackerUnits[0].TakeHit();
        attackerUnits[1].TakeHit();
        var remainingAttacker = attackerUnits.Where(u => u.isAlive).ToList();
        
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            remainingAttacker,
            new List<Unit>());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerAvgCpLoss.Should().BeGreaterThan(0);
    }
    
    #endregion

    #region Remaining Units Average Tests
    
    [Fact]
    public void AttackerRemainingUnitsAvg_WithNoResults_ShouldHaveZeroUnits()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.AttackerRemainingUnitsAvg.InfantryUnits.Should().Be(0);
        stats.AttackerRemainingUnitsAvg.TankUnits.Should().Be(0);
        stats.AttackerRemainingUnitsAvg.ArtilleryUnits.Should().Be(0);
    }

    [Fact]
    public void DefenderRemainingUnitsAvg_WithNoResults_ShouldHaveZeroUnits()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.DefenderRemainingUnitsAvg.InfantryUnits.Should().Be(0);
        stats.DefenderRemainingUnitsAvg.TankUnits.Should().Be(0);
        stats.DefenderRemainingUnitsAvg.ArtilleryUnits.Should().Be(0);
    }

    [Fact]
    public void AttackerRemainingUnitsAvg_ShouldBeUnitsStatsType()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.AttackerRemainingUnitsAvg.Should().BeOfType<UnitsStats>();
    }

    [Fact]
    public void DefenderRemainingUnitsAvg_ShouldBeUnitsStatsType()
    {
        // Arrange
        var stats = new SimulationStats();

        // Assert
        stats.DefenderRemainingUnitsAvg.Should().BeOfType<UnitsStats>();
    }
    
    #endregion

    #region Army Properties Tests
    
    [Fact]
    public void AttackingArmy_ShouldBeSettable()
    {
        // Arrange
        var stats = new SimulationStats();
        var army = new LandArmy(isAttacking: true, infantryCount: 5);

        // Act
        stats.AttackingArmy = army;

        // Assert
        stats.AttackingArmy.Should().BeSameAs(army);
    }

    [Fact]
    public void DefendingArmy_ShouldBeSettable()
    {
        // Arrange
        var stats = new SimulationStats();
        var army = new LandArmy(isAttacking: false, infantryCount: 5);

        // Act
        stats.DefendingArmy = army;

        // Assert
        stats.DefendingArmy.Should().BeSameAs(army);
    }
    
    #endregion

    #region Edge Case Tests
    
    [Fact]
    public void Percentage_WithSingleBattle_ShouldBe100OrZero()
    {
        // Arrange
        var stats = new SimulationStats { AttackerWon = 1 };

        // Assert
        stats.AttackerWonPercentage.Should().Be(100.0);
        stats.DefenderWonPercentage.Should().Be(0);
        stats.DrawPercentage.Should().Be(0);
    }

    [Fact]
    public void RecordResult_WithFullArmyRemaining_ShouldTrackCorrectly()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3, tankCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 1);
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.AttackerVictory,
            attackingArmy.GetAllAliveUnits(),
            new List<Unit>());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerRemainingUnitsAvg.InfantryUnits.Should().Be(3);
        stats.AttackerRemainingUnitsAvg.TankUnits.Should().Be(2);
    }

    [Fact]
    public void RecordResult_WithNoUnitsRemaining_ShouldTrackCorrectly()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var result = new BattleResult(
            attackingArmy,
            defendingArmy,
            BattleOutcome.Draw,
            new List<Unit>(),
            new List<Unit>());

        // Act
        stats.RecordResult(result);

        // Assert
        stats.AttackerRemainingUnitsAvg.InfantryUnits.Should().Be(0);
        stats.DefenderRemainingUnitsAvg.InfantryUnits.Should().Be(0);
    }
    
    #endregion

    #region Integration Tests
    
    [Fact]
    public void SimulationStats_AfterMultipleBattles_ShouldCalculateAveragesCorrectly()
    {
        // Arrange
        var stats = new SimulationStats();
        var attackingArmy1 = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy1 = new LandArmy(isAttacking: false, infantryCount: 1);
        
        // First battle - attacker wins with 4 remaining
        var remaining1 = attackingArmy1.GetAllAliveUnits().Take(4).ToList();
        var result1 = new BattleResult(attackingArmy1, defendingArmy1, BattleOutcome.AttackerVictory, 
            remaining1, new List<Unit>());
        
        // Second battle - attacker wins with 2 remaining
        var attackingArmy2 = new LandArmy(isAttacking: true, infantryCount: 5);
        var defendingArmy2 = new LandArmy(isAttacking: false, infantryCount: 1);
        var remaining2 = attackingArmy2.GetAllAliveUnits().Take(2).ToList();
        var result2 = new BattleResult(attackingArmy2, defendingArmy2, BattleOutcome.AttackerVictory, 
            remaining2, new List<Unit>());

        // Act
        stats.RecordResult(result1);
        stats.RecordResult(result2);

        // Assert
        stats.AttackerWon.Should().Be(2);
        stats.TotalBattles.Should().Be(2);
        stats.AttackerWonPercentage.Should().Be(100.0);
        stats.AttackerRemainingUnitsAvg.InfantryUnits.Should().Be(3); // Average of 4 and 2
    }
    
    #endregion
}

