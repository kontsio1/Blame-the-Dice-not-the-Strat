using axis_console_project.UnitTypes.Land;
using axis_console_project.UnitTypes.Sea;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests;

public class NavalArmadaTests
{
    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, 1, 2, 3, 4, 5, 6, 7, 8);

        // Assert
        Assert.True(army.isAttacking);
        army.units.InfantryUnits.Should().HaveCount(0);
        army.units.ArtilleryUnits.Should().HaveCount(0);
        army.units.TankUnits.Should().HaveCount(0);
        army.units.FighterUnits.Should().HaveCount(7);
        army.units.BomberUnits.Should().HaveCount(8);
        army.units.AntiAirUnits.Should().HaveCount(0);

        army.units.TransportUnits.Should().HaveCount(1);
        army.units.SubmarineUnits.Should().HaveCount(2);
        army.units.DestroyerUnits.Should().HaveCount(3);
        army.units.CruiserUnits.Should().HaveCount(4);
        army.units.BattleshipUnits.Should().HaveCount(5);
        army.units.AircraftCarrierUnits.Should().HaveCount(6);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectTransportUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, transportCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.TransportUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectSubmarineUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, submarineCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.SubmarineUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectDestroyerUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, destroyerCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.DestroyerUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectCruiserUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, cruiserCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.CruiserUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectBattleshipUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, battleshipCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.BattleshipUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectAircraftCarrierUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, carrierCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.AircraftCarrierUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectFighterCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, fighterCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.FighterUnits.Should().HaveCount(3);
    }

    [Fact]
    public void ArmadaShouldCreateAnArmyWithCorrectBomberUnitCounts()
    {
        // Arrange
        // Act
        var army = new NavalArmada(true, bomberCount: 3);

        // Assert
        Assert.True(army.isAttacking);
        army.units.BomberUnits.Should().HaveCount(3);
    }
}
