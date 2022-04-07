using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Communication;


public class OutboundMessagingConfig : List<OutboundMessagingConfig.OutboundEndpointConfig>
{
    public const string Name = "OutboundMessages";

    public class OutboundEndpointConfig
    {
        public MessageEndpointConfig Endpoint { get; set; } = null!;
        public IList<string> Messages { get; set; } = null!;
    }
}

internal class OutboundMessagingConfigurationBinder : IConfigureOptions<OutboundMessagingConfig>, IValidateOptions<OutboundMessagingConfig>
{
    private readonly IConfiguration _configuration;

    public OutboundMessagingConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(OutboundMessagingConfig options) => _configuration.GetSection(OutboundMessagingConfig.Name).Bind(options);

    public ValidateOptionsResult Validate(string name, OutboundMessagingConfig options)
    {
        if (options is null) return ValidateOptionsResult.Skip;
        if (options.Count == 0) return ValidateOptionsResult.Skip;

        var errors = new List<string>();

        foreach (var item in options)
        {
            if (item.Messages.Count == 0) errors.Add("No Messages registered for endpoint");
            if (string.IsNullOrWhiteSpace(item.Endpoint.QueueName)) errors.Add("QueueName is empty");
            if (string.IsNullOrWhiteSpace(item.Endpoint.Host)) errors.Add("Host is empty");
            if (string.IsNullOrWhiteSpace(item.Endpoint.RoutingKey)) errors.Add("Routing Key is empty");
        }

        if (errors.Count == 0) return ValidateOptionsResult.Success;

        return ValidateOptionsResult.Fail(errors);
    }
}
