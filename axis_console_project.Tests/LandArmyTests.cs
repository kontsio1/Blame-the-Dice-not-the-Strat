using System.Reflection;
using axis_console_project.UnitTypes;
using axis_console_project.UnitTypes.Land;
using FluentAssertions;
using Xunit;

namespace axis_console_project.Tests;

public class LandArmyTests
{
    [Fact]
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
}
