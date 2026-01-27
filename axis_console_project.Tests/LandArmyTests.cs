using System.Reflection;
using axis_console_project.BaseClasses;
using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Air;
using axis_console_project.UnitTypes.Land;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests;

public class LandArmyTests
{
    public void LandArmyShouldCreateAnArmyWithCorrectUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, 1, 2, 3, 4, 5, 6, 7, 8);

        // Assert
        Assert.True(army.isAttacking);
        army.units.InfantryUnits.Should().HaveCount(1);
        army.units.ArtilleryUnits.Should().HaveCount(2);
        army.units.TankUnits.Should().HaveCount(3);
        army.units.FighterUnits.Should().HaveCount(4);
        army.units.BomberUnits.Should().HaveCount(5);
        army.units.AntiAirUnits.Should().HaveCount(6);
        army.units.CruiserUnits.Should().HaveCount(7);
        army.units.BattleshipUnits.Should().HaveCount(8);

        army.units.TransportUnits.Should().HaveCount(0);
        army.units.SubmarineUnits.Should().HaveCount(0);
        army.units.DestroyerUnits.Should().HaveCount(0);
        army.units.AircraftCarrierUnits.Should().HaveCount(0);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectInfantryUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, infantryCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.InfantryUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectArtilleryUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, artilleryCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.ArtilleryUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectTankUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, tankCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.TankUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectFighterUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, fighterCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.FighterUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectBomberUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, bomberCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.BomberUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectAntiAirUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, antiAirCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.AntiAirUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectCruiserUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, cruiserCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.CruiserUnits.Should().HaveCount(3);
    }

    [Fact]
    public void LandArmyShouldCreateAnArmyWithCorrectBattleshipUnitCounts()
    {
        // Arrange
        // Act
        var army = new LandArmy(true, battleshipCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.BattleshipUnits.Should().HaveCount(3);
    }

    [Fact]
    public void InfantryShouldHaveIncreasedAttackWhenAccompaniedByArtillery()
    {
        // Arrange
        var army = new LandArmy(true, 5, 2);

        // Act
        var infantriesWithArtillery = army
            .units.InfantryUnits.Where(i =>
            {
                var propertyInfo = typeof(Unit).GetProperty(
                    "Attack",
                    BindingFlags.NonPublic | BindingFlags.Instance
                );
                var attackValue = (int)propertyInfo.GetValue(i);

                return i.AccompaniedByArtillery && attackValue == 2;
            })
            .ToList();

        // Assert
        infantriesWithArtillery.Should().HaveCount(2);
    }

    [Fact]
    public void AntiairShouldDefendAgainstAttackingAirUnits()
    {
        // Arrange
        var numberOfSims = 1000;

        var attackingArmy = new LandArmy(true, fighterCount: 10, bomberCount: 5);

        var exodiaAntiAir = new AntiAir(false);
        var propertyInfo = typeof(Unit).GetProperty(
            "Attack",
            BindingFlags.NonPublic | BindingFlags.Instance
        );
        var standardAntiAirAttack = propertyInfo.GetValue(new AntiAir(false), null);
        propertyInfo.SetValue(exodiaAntiAir, 6);

        var defendingArmy = new LandArmy
        {
            isAttacking = false,
            units = new Units { IsAttacking = false, AntiAirUnits = [exodiaAntiAir] },
        };
        // Act
        var simStats = new Simulation(attackingArmy, defendingArmy, numberOfSims).Run();
        // Assert
        Assert.Equal(numberOfSims, simStats.TotalBattles);
        Assert.Equal(1, (double)simStats.AttackerWonPercentage / 100, 2);
        Assert.Equal(0, (double)simStats.DefenderWonPercentage / 100, 2);
        Assert.Equal(1, standardAntiAirAttack);
    }

    [Fact]
    public void BattleResultShouldBeWithinAcceptableRange1()
    {
        // Arrange
        var numberOfSims = 10000;
        var attackingArmy = new LandArmy(true, tankCount: 5);
        var defendingArmy = new LandArmy(false, tankCount: 5);

        // Act
        var simStats = new Simulation(attackingArmy, defendingArmy, numberOfSims).Run();
        double attackerWonOrDraw =
            (simStats.AttackerWonPercentage + simStats.DrawPercentage / 2) / 100;
        double defenderWonOrDraw =
            (simStats.DefenderWonPercentage + simStats.DrawPercentage / 2) / 100;
        // Assert
        Assert.Equal(numberOfSims, simStats.TotalBattles);
        Assert.Equal(0.50, attackerWonOrDraw, 2); // 1 percent accuracy
        Assert.Equal(0.50, defenderWonOrDraw, 2);
    }

    [Fact]
    public void BattleResultShouldBeWithinAcceptableRange2()
    {
        // Arrange
        var numberOfSims = 10000;
        var attackingArmy = new LandArmy(true, 1, 2, 3);
        var defendingArmy = new LandArmy(false, 1, 2, 3);

        // Act
        var simStats = new Simulation(attackingArmy, defendingArmy, numberOfSims).Run();
        double attackerWonOrDraw =
            (simStats.AttackerWonPercentage + simStats.DrawPercentage / 2) / 100;
        double defenderWonOrDraw =
            (simStats.DefenderWonPercentage + simStats.DrawPercentage / 2) / 100;
        // Assert
        Assert.Equal(numberOfSims, simStats.TotalBattles);
        Assert.Equal(0.50, attackerWonOrDraw, 2); // 1 percent accuracy
        Assert.Equal(0.50, defenderWonOrDraw, 2);
    }
}
