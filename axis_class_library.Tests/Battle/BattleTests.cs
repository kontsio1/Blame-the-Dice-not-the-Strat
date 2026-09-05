using axis_console_project.Battles;
using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Battle;

public class BattleTests
{
    #region Constructor Tests
    
    [Fact]
    public void Constructor_ShouldSetAttackingArmy()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);

        // Act
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Assert
        battle.AttackingArmy.Should().BeSameAs(attackingArmy);
    }

    [Fact]
    public void Constructor_ShouldSetDefendingArmy()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);

        // Act
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Assert
        battle.DefendingArmy.Should().BeSameAs(defendingArmy);
    }

    [Fact]
    public void Constructor_ShouldHaveNullOutcome_Initially()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);

        // Act
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Assert
        battle.Outcome.Should().BeNull();
    }
    
    #endregion

    #region Fight Method Tests
    
    [Fact]
    public void Fight_ShouldReturnBattleResult()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BattleResult>();
    }

    [Fact]
    public void Fight_ShouldSetOutcome()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        battle.Fight();

        // Assert
        battle.Outcome.Should().NotBeNull();
        battle.Outcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }

    [Fact]
    public void Fight_WithOverwhelmingAttacker_ShouldLikelyResultInAttackerVictory()
    {
        // Arrange - Run multiple battles to check statistical outcome
        var attackerWins = 0;
        const int iterations = 100;

        for (int i = 0; i < iterations; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 10, tankCount: 5);
            var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 1);
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                attackerWins++;
        }

        // Assert - With overwhelming force, attacker should win most battles
        attackerWins.Should().BeGreaterThan(80); // At least 80% win rate
    }

    [Fact]
    public void Fight_WithOverwhelmingDefender_ShouldLikelyResultInDefenderVictory()
    {
        // Arrange - Run multiple battles to check statistical outcome
        var defenderWins = 0;
        const int iterations = 100;

        for (int i = 0; i < iterations; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 1);
            var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 10, tankCount: 5);
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();

            if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                defenderWins++;
        }

        // Assert - With overwhelming force, defender should win most battles
        defenderWins.Should().BeGreaterThan(80); // At least 80% win rate
    }

    [Fact]
    public void Fight_ShouldReturnRemainingUnitsForBothSides()
    {
        // Arrange
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.AttackerRemainingUnits.Should().NotBeNull();
        result.DefenderRemainingUnits.Should().NotBeNull();
    }

    [Fact]
    public void Fight_WhenAttackerWins_DefenderShouldHaveNoAliveUnits()
    {
        // Arrange & Act - Run battles until we get an attacker victory
        BattleResult? result = null;
        for (int i = 0; i < 1000; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5, tankCount: 3);
            var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 2);
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            result = battle.Fight();
            
            if (result.BattleOutcome == BattleOutcome.AttackerVictory)
                break;
        }

        // Assert
        result.Should().NotBeNull();
        result!.BattleOutcome.Should().Be(BattleOutcome.AttackerVictory);
        result.DefenderRemainingUnits.GetAllUnits().Where(u => u.isAlive).Should().BeEmpty();
    }

    [Fact]
    public void Fight_WhenDefenderWins_AttackerShouldHaveNoAliveUnits()
    {
        // Arrange & Act - Run battles until we get a defender victory
        BattleResult? result = null;
        for (int i = 0; i < 1000; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 2);
            var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5, tankCount: 3);
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            result = battle.Fight();
            
            if (result.BattleOutcome == BattleOutcome.DefenderVictory)
                break;
        }

        // Assert
        result.Should().NotBeNull();
        result!.BattleOutcome.Should().Be(BattleOutcome.DefenderVictory);
        result.AttackerRemainingUnits.GetAllUnits().Where(u => u.isAlive).Should().BeEmpty();
    }
    
    #endregion

    #region Naval Battle Tests
    
    [Fact]
    public void Fight_NavalBattle_ShouldComplete()
    {
        // Arrange
        var attackingFleet = new NavalArmada(isAttacking: true, destroyerCount: 3, submarineCount: 2);
        var defendingFleet = new NavalArmada(isAttacking: false, destroyerCount: 2, cruiserCount: 1);
        var battle = new Battles.Battle(attackingFleet, defendingFleet);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }

    [Fact]
    public void Fight_NavalBattle_WithSubmarineSupriseAttack_ShouldHappenWhenNoDestroyers()
    {
        // Arrange - Attacker has subs, defender has no destroyers or subs
        var attackingFleet = new NavalArmada(isAttacking: true, submarineCount: 5);
        var defendingFleet = new NavalArmada(isAttacking: false, cruiserCount: 2);
        var battle = new Battles.Battle(attackingFleet, defendingFleet);

        // Act
        var result = battle.Fight();

        // Assert - Battle should complete successfully
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }

    [Fact]
    public void Fight_NavalBattle_DestroyersShouldPreventSubSurpriseAttack()
    {
        // Arrange - Defender has destroyers which should prevent surprise attack
        var attackingFleet = new NavalArmada(isAttacking: true, submarineCount: 3);
        var defendingFleet = new NavalArmada(isAttacking: false, destroyerCount: 2);
        
        // Just verify the property works correctly
        defendingFleet.PreventsSubSupriseAttack.Should().BeTrue();
    }

    [Fact]
    public void Fight_NavalBattle_SubmarinesShouldPreventSubSurpriseAttack()
    {
        // Arrange - Defender has submarines which should also prevent surprise attack
        var attackingFleet = new NavalArmada(isAttacking: true, submarineCount: 3);
        var defendingFleet = new NavalArmada(isAttacking: false, submarineCount: 2);
        
        // Just verify the property works correctly
        defendingFleet.PreventsSubSupriseAttack.Should().BeTrue();
    }
    
    #endregion

    #region Land Battle with Special Mechanics Tests
    
    [Fact]
    public void Fight_LandBattle_WithNavalBombardment_ShouldComplete()
    {
        // Arrange - Land army with naval support
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 5, cruiserCount: 2, battleshipCount: 1);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }

    [Fact]
    public void Fight_LandBattle_WithAntiAirDefense_ShouldComplete()
    {
        // Arrange - Attacker has air units, defender has anti-air
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3, fighterCount: 2, bomberCount: 1);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 5, antiAirCount: 1);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }
    
    #endregion

    #region Edge Cases
    
    [Fact]
    public void Fight_WithMinimalForces_ShouldComplete()
    {
        // Arrange - Single unit on each side
        var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 1);
        var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 1);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }

    [Fact]
    public void Fight_MultipleBattles_ShouldProduceDifferentOutcomes()
    {
        // Arrange - Equal forces should produce varied outcomes
        var outcomes = new HashSet<BattleOutcome>();
        
        for (int i = 0; i < 100; i++)
        {
            var attackingArmy = new LandArmy(isAttacking: true, infantryCount: 3, tankCount: 2);
            var defendingArmy = new LandArmy(isAttacking: false, infantryCount: 3, tankCount: 2);
            var battle = new Battles.Battle(attackingArmy, defendingArmy);
            var result = battle.Fight();
            outcomes.Add(result.BattleOutcome);
        }

        // Assert - With equal forces, we should see multiple different outcomes
        outcomes.Count.Should().BeGreaterThan(1);
    }

    [Fact]
    public void Fight_WithMixedUnitTypes_ShouldComplete()
    {
        // Arrange - Mix of land and air units
        var attackingArmy = new LandArmy(
            isAttacking: true, 
            infantryCount: 2, 
            artilleryCount: 1, 
            tankCount: 2, 
            fighterCount: 1, 
            bomberCount: 1);
        var defendingArmy = new LandArmy(
            isAttacking: false, 
            infantryCount: 3, 
            artilleryCount: 1, 
            tankCount: 1, 
            fighterCount: 2);
        var battle = new Battles.Battle(attackingArmy, defendingArmy);

        // Act
        var result = battle.Fight();

        // Assert
        result.Should().NotBeNull();
        result.BattleOutcome.Should().BeOneOf(BattleOutcome.AttackerVictory, BattleOutcome.DefenderVictory, BattleOutcome.Draw);
    }
    
    #endregion
}

