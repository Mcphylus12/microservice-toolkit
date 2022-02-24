using Communication.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Communication;

internal class ConfigurationBinder : IConfigureOptions<CommunicationConfiguration>
{
    private readonly IConfiguration _configuration;

    public ConfigurationBinder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CommunicationConfiguration options)
    {
        _configuration.GetSection(CommunicationConfiguration.Name).Bind(options);
    }
}
