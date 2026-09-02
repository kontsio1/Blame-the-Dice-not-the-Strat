namespace axis_mobile_app.PageModels;

public record OutcomeRow(string Result, int Count, string Percentage);

public record ComparisonRow(string Unit, string Attacker, string Defender);

public record MetricRow(string Metric, string Value);

