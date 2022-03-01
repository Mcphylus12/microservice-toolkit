using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Communication;

public class InboundMessagingConfig : List<MessageEndpointConfig>
{
    public const string Name = "InboundMessages";
}

internal class InboundMessagingConfigurationBinder : IConfigureOptions<InboundMessagingConfig>, IValidateOptions<InboundMessagingConfig>
{
    private readonly IConfiguration _configuration;

    public InboundMessagingConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(InboundMessagingConfig options) => _configuration.GetSection(InboundMessagingConfig.Name).Bind(options);

    public ValidateOptionsResult Validate(string name, InboundMessagingConfig options)
    {
        if (options is null) return ValidateOptionsResult.Skip;
        if (options.Count == 0) return ValidateOptionsResult.Skip;

        var errors = new List<string>();

        foreach (var item in options)
        {
            if (string.IsNullOrWhiteSpace(item.QueueName)) errors.Add("QueueName is empty");
            if (string.IsNullOrWhiteSpace(item.Host)) errors.Add("Host is empty");
        }

        if (errors.Count == 0) return ValidateOptionsResult.Success;

        return ValidateOptionsResult.Fail(errors);
    }
}
