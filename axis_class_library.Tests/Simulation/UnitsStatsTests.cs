using axis_console_project.Simulations;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests.Simulation;

public class UnitsStatsTests
{
    #region Constructor Tests
    
    [Fact]
    public void Constructor_WithDefaultValues_ShouldSetAllToZero()
    {
        // Act
        var stats = new UnitsStats();

        // Assert
        stats.InfantryUnits.Should().Be(0);
        stats.ArtilleryUnits.Should().Be(0);
        stats.TankUnits.Should().Be(0);
        stats.AntiAirUnits.Should().Be(0);
        stats.FighterUnits.Should().Be(0);
        stats.BomberUnits.Should().Be(0);
        stats.TransportUnits.Should().Be(0);
        stats.SubmarineUnits.Should().Be(0);
        stats.DestroyerUnits.Should().Be(0);
        stats.CruiserUnits.Should().Be(0);
        stats.BattleshipUnits.Should().Be(0);
        stats.AircraftCarrierUnits.Should().Be(0);
    }

    [Fact]
    public void Constructor_WithSpecificValues_ShouldSetCorrectly()
    {
        // Act
        var stats = new UnitsStats(
            infantryUnits: 5.5,
            artilleryUnits: 2.3,
            tankUnits: 3.7,
            antiAirUnits: 1.0,
            fighterUnits: 2.1);

        // Assert
        stats.InfantryUnits.Should().Be(5.5);
        stats.ArtilleryUnits.Should().Be(2.3);
        stats.TankUnits.Should().Be(3.7);
        stats.AntiAirUnits.Should().Be(1.0);
        stats.FighterUnits.Should().Be(2.1);
    }

    [Fact]
    public void Constructor_WithNavalUnits_ShouldSetCorrectly()
    {
        // Act
        var stats = new UnitsStats(
            submarineUnits: 3.2,
            destroyerUnits: 2.5,
            cruiserUnits: 1.8,
            battleshipUnits: 0.5,
            aircraftCarrierUnits: 1.2);

        // Assert
        stats.SubmarineUnits.Should().Be(3.2);
        stats.DestroyerUnits.Should().Be(2.5);
        stats.CruiserUnits.Should().Be(1.8);
        stats.BattleshipUnits.Should().Be(0.5);
        stats.AircraftCarrierUnits.Should().Be(1.2);
    }

    [Fact]
    public void Constructor_WithDecimalValues_ShouldPreservePrecision()
    {
        // Act
        var stats = new UnitsStats(
            infantryUnits: 3.14159,
            tankUnits: 2.71828);

        // Assert
        stats.InfantryUnits.Should().BeApproximately(3.14159, 0.00001);
        stats.TankUnits.Should().BeApproximately(2.71828, 0.00001);
    }
    
    #endregion

    #region Property Tests
    
    [Fact]
    public void InfantryUnits_ShouldBeModifiable()
    {
        // Arrange
        var stats = new UnitsStats();

        // Act
        stats.InfantryUnits = 10.5;

        // Assert
        stats.InfantryUnits.Should().Be(10.5);
    }

    [Fact]
    public void AllProperties_ShouldBeModifiable()
    {
        // Arrange
        var stats = new UnitsStats();

        // Act
        stats.InfantryUnits = 1;
        stats.ArtilleryUnits = 2;
        stats.TankUnits = 3;
        stats.AntiAirUnits = 4;
        stats.FighterUnits = 5;
        stats.BomberUnits = 6;
        stats.TransportUnits = 7;
        stats.SubmarineUnits = 8;
        stats.DestroyerUnits = 9;
        stats.CruiserUnits = 10;
        stats.BattleshipUnits = 11;
        stats.AircraftCarrierUnits = 12;

        // Assert
        stats.InfantryUnits.Should().Be(1);
        stats.ArtilleryUnits.Should().Be(2);
        stats.TankUnits.Should().Be(3);
        stats.AntiAirUnits.Should().Be(4);
        stats.FighterUnits.Should().Be(5);
        stats.BomberUnits.Should().Be(6);
        stats.TransportUnits.Should().Be(7);
        stats.SubmarineUnits.Should().Be(8);
        stats.DestroyerUnits.Should().Be(9);
        stats.CruiserUnits.Should().Be(10);
        stats.BattleshipUnits.Should().Be(11);
        stats.AircraftCarrierUnits.Should().Be(12);
    }
    
    #endregion

    #region ToString Tests
    
    [Fact]
    public void ToString_WithAllZeros_ShouldReturnEmptyOrMinimal()
    {
        // Arrange
        var stats = new UnitsStats();

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty(); // No units to display
    }

    [Fact]
    public void ToString_WithInfantry_ShouldIncludeInfantry()
    {
        // Arrange
        var stats = new UnitsStats(infantryUnits: 5.5);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Infantry");
        result.Should().Contain("5.50");
    }

    [Fact]
    public void ToString_WithTanks_ShouldIncludeTanks()
    {
        // Arrange
        var stats = new UnitsStats(tankUnits: 3.25);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Tanks");
        result.Should().Contain("3.25");
    }

    [Fact]
    public void ToString_WithMultipleUnits_ShouldIncludeAll()
    {
        // Arrange
        var stats = new UnitsStats(
            infantryUnits: 5,
            artilleryUnits: 2,
            tankUnits: 3);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Infantry");
        result.Should().Contain("Artillery");
        result.Should().Contain("Tanks");
    }

    [Fact]
    public void ToString_ShouldNotIncludeZeroValueUnits()
    {
        // Arrange
        var stats = new UnitsStats(infantryUnits: 5);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Infantry");
        result.Should().NotContain("Artillery");
        result.Should().NotContain("Tanks");
    }

    [Fact]
    public void ToString_WithNavalUnits_ShouldIncludeNavalUnits()
    {
        // Arrange
        var stats = new UnitsStats(
            submarineUnits: 2,
            destroyerUnits: 3,
            battleshipUnits: 1);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Submarines");
        result.Should().Contain("Destroyers");
        result.Should().Contain("Battleships");
    }

    [Fact]
    public void ToString_WithAirUnits_ShouldIncludeAirUnits()
    {
        // Arrange
        var stats = new UnitsStats(
            fighterUnits: 4,
            bomberUnits: 2);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Fighters");
        result.Should().Contain("Bombers");
    }

    [Fact]
    public void ToString_WithAntiAir_ShouldIncludeAntiAir()
    {
        // Arrange
        var stats = new UnitsStats(antiAirUnits: 1);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("AntiAir");
    }

    [Fact]
    public void ToString_WithTransports_ShouldIncludeTransports()
    {
        // Arrange
        var stats = new UnitsStats(transportUnits: 2);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Transports");
    }

    [Fact]
    public void ToString_WithCruisers_ShouldIncludeCruisers()
    {
        // Arrange
        var stats = new UnitsStats(cruiserUnits: 3);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Cruisers");
    }

    [Fact]
    public void ToString_WithAircraftCarriers_ShouldIncludeAircraftCarriers()
    {
        // Arrange
        var stats = new UnitsStats(aircraftCarrierUnits: 2);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("AircraftCarriers");
    }
    
    #endregion

    #region Formatting Tests
    
    [Fact]
    public void ToString_ShouldFormatWithTwoDecimalPlaces()
    {
        // Arrange
        var stats = new UnitsStats(infantryUnits: 5.123);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("5.12"); // Formatted to 2 decimal places
    }

    [Fact]
    public void ToString_WithWholeNumbers_ShouldShowTwoDecimalPlaces()
    {
        // Arrange
        var stats = new UnitsStats(tankUnits: 3);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("3.00");
    }

    [Fact]
    public void ToString_MultipleItems_ShouldBeSeparatedByCommaNewline()
    {
        // Arrange
        var stats = new UnitsStats(infantryUnits: 1, tankUnits: 2);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain(",");
    }
    
    #endregion

    #region Edge Cases
    
    [Fact]
    public void Constructor_WithNegativeValues_ShouldAccept()
    {
        // This tests edge case behavior - negative values shouldn't normally occur
        // but the class should handle them gracefully
        
        // Act
        var stats = new UnitsStats(infantryUnits: -1);

        // Assert
        stats.InfantryUnits.Should().Be(-1);
    }

    [Fact]
    public void Constructor_WithVeryLargeValues_ShouldAccept()
    {
        // Act
        var stats = new UnitsStats(infantryUnits: 1000000.99);

        // Assert
        stats.InfantryUnits.Should().Be(1000000.99);
    }

    [Fact]
    public void Constructor_WithVerySmallValues_ShouldAccept()
    {
        // Act
        var stats = new UnitsStats(infantryUnits: 0.01);

        // Assert
        stats.InfantryUnits.Should().Be(0.01);
    }

    [Fact]
    public void ToString_WithAllUnitTypes_ShouldIncludeAll()
    {
        // Arrange
        var stats = new UnitsStats(
            infantryUnits: 1,
            artilleryUnits: 2,
            tankUnits: 3,
            antiAirUnits: 4,
            fighterUnits: 5,
            bomberUnits: 6,
            transportUnits: 7,
            submarineUnits: 8,
            destroyerUnits: 9,
            cruiserUnits: 10,
            battleshipUnits: 11,
            aircraftCarrierUnits: 12);

        // Act
        var result = stats.ToString();

        // Assert
        result.Should().Contain("Infantry");
        result.Should().Contain("Artillery");
        result.Should().Contain("Tanks");
        result.Should().Contain("AntiAir");
        result.Should().Contain("Fighters");
        result.Should().Contain("Bombers");
        result.Should().Contain("Transports");
        result.Should().Contain("Submarines");
        result.Should().Contain("Destroyers");
        result.Should().Contain("Cruisers");
        result.Should().Contain("Battleships");
        result.Should().Contain("AircraftCarriers");
    }
    
    #endregion
}

