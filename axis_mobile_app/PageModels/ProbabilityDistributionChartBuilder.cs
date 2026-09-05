using axis_console_project.Simulations;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace axis_mobile_app.PageModels;

public sealed record ProbabilityDistributionChartData(
    ISeries[] Series,
    Axis[] XAxes,
    Axis[] YAxes
);

public static class ProbabilityDistributionChartBuilder
{
    public static ProbabilityDistributionChartData Empty()
    {
        return new ProbabilityDistributionChartData(
            Series: [],
            XAxes: [BuildXAxis([])],
            YAxes: [BuildYAxis()]
        );
    }

    public static ProbabilityDistributionChartData Build(SimulationStats? stats)
    {
        if (stats is null || stats.BattleResults.Count == 0)
        {
            return Empty();
        }

        var totalBattles = stats.BattleResults.Count;

        var probabilityByDelta = stats.BattleResults
            .Select(result => result.AttackerRemainingUnits.Cost - result.DefenderRemainingUnits.Cost)
            .GroupBy(value => value)
            .ToDictionary(group => group.Key, group => (group.Count() * 100.0) / totalBattles);

        // Keep only observed deltas so the line connects real points and does not drop to 0 for gaps.
        var deltas = probabilityByDelta.Keys
            .OrderByDescending(delta => delta)
            .ToList();

        var labels = deltas
            .Select(FormatSigned)
            .ToArray();

        var attackerValues = new double?[deltas.Count];
        var defenderValues = new double?[deltas.Count];

        for (var i = 0; i < deltas.Count; i++)
        {
            var delta = deltas[i];
            var probability = probabilityByDelta[delta];

            if (delta < 0)
            {
                defenderValues[i] = probability;
            }

            if (delta > 0)
            {
                attackerValues[i] = probability;
            }

            if (delta == 0)
            {
                // Share the 0 point so the attacker and defender segments join seamlessly.
                attackerValues[i] = probability;
                defenderValues[i] = probability;
            }
        }

        var series = new ISeries[]
        {
            new LineSeries<double?>
            {
                Name = "Attacker",
                Values = attackerValues,
                Fill = null,
                Stroke = new SolidColorPaint(new SKColor(70, 130, 180), 3),
                GeometryFill = new SolidColorPaint(new SKColor(70, 130, 180)),
                GeometryStroke = null,
                GeometrySize = 6
            },
            new LineSeries<double?>
            {
                Name = "Defender",
                Values = defenderValues,
                Fill = null,
                Stroke = new SolidColorPaint(new SKColor(205, 92, 92), 3),
                GeometryFill = new SolidColorPaint(new SKColor(205, 92, 92)),
                GeometryStroke = null,
                GeometrySize = 6
            }
        };

        return new ProbabilityDistributionChartData(
            Series: series,
            XAxes: [BuildXAxis(labels)],
            YAxes: [BuildYAxis()]
        );
    }

    private static Axis BuildXAxis(string[] labels)
    {
        return new Axis
        {
            Name = "IPC delta (Attacker - Defender)",
            Labels = labels,
            LabelsRotation = 35
        };
    }

    private static Axis BuildYAxis()
    {
        return new Axis
        {
            Name = "Probability",
            Labeler = value => $"{value:0.#}%",
            MinLimit = 0
        };
    }

    private static string FormatSigned(int value) => value > 0 ? $"+{value}" : value.ToString();
}

