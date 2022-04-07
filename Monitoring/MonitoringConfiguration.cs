using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Monitoring;

public class MonitoringConfiguration
{
    public const string Name = "Monitoring";
    public Dictionary<string, List<double>> HistogramBins { get; set; } = null!;
}

internal class MonitoringConfigurationBinder : IConfigureOptions<MonitoringConfiguration>, IValidateOptions<MonitoringConfiguration>
{
    private readonly IConfiguration _configuration;

    public MonitoringConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(MonitoringConfiguration options) => _configuration.GetSection(MonitoringConfiguration.Name).Bind(options);

    public ValidateOptionsResult Validate(string name, MonitoringConfiguration options)
    {
        if (options?.HistogramBins is null) return ValidateOptionsResult.Skip;
        if (options.HistogramBins.Count == 0) return ValidateOptionsResult.Skip;
        var errors = new List<string>();

        foreach (var bins in options.HistogramBins)
        {
            if (bins.Value.Count == 0) errors.Add($"no bins defined for {bins.Key}");
        }

        if (errors.Count == 0) return ValidateOptionsResult.Success;

        return ValidateOptionsResult.Fail(errors);
    }
}
