using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Communication;

public class OutboundRequestConfig : List<OutboundRequestConfig.RequestEndpoint>
{
    public const string Name = "OutboundRequests";

    public class RequestEndpoint
    {
        public string Endpoint { get; set; }
        public IList<string> Requests { get; set; }
    }
}

internal class OutboundRequestConfigurationBinder : IConfigureOptions<OutboundRequestConfig>, IValidateOptions<OutboundRequestConfig>
{
    private readonly IConfiguration _configuration;

    public OutboundRequestConfigurationBinder(IConfiguration configuration) => _configuration = configuration;
    public void Configure(OutboundRequestConfig options) => _configuration.GetSection(OutboundRequestConfig.Name).Bind(options);

    public ValidateOptionsResult Validate(string name, OutboundRequestConfig options)
    {
        if (options is null) return ValidateOptionsResult.Skip;
        if (options.Count == 0) return ValidateOptionsResult.Skip;

        var errors = new List<string>();

        foreach (var item in options)
        {
            if (item.Requests.Count == 0) errors.Add("No Requests registered for endpoint");
            if (string.IsNullOrWhiteSpace(item.Endpoint)) errors.Add("Endpoint is empty");
        }

        if (errors.Count == 0) return ValidateOptionsResult.Success;

        return ValidateOptionsResult.Fail(errors);
    }
}
