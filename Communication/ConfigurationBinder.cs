using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Communication;

internal class InboundMessagingConfigurationBinder : IConfigureOptions<InboundMessagingConfig>
{
    private readonly IConfiguration _configuration;

    public InboundMessagingConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(InboundMessagingConfig options) => _configuration.GetSection(InboundMessagingConfig.Name).Bind(options);
}

internal class OutboundMessagingConfigurationBinder : IConfigureOptions<OutboundMessagingConfig>
{
    private readonly IConfiguration _configuration;

    public OutboundMessagingConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(OutboundMessagingConfig options) => _configuration.GetSection(OutboundMessagingConfig.Name).Bind(options);
}

internal class OutboundRequestConfigurationBinder : IConfigureOptions<OutboundRequestConfig>
{
    private readonly IConfiguration _configuration;

    public OutboundRequestConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(OutboundRequestConfig options) => _configuration.GetSection(OutboundRequestConfig.Name).Bind(options);
}
