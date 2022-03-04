using Microsoft.Extensions.Configuration;
using Polly;

namespace Configuration;

public static class ConfigurationSourceExtensions
{
    public static void AddApiSource(this ConfigurationManager builder)
    {
        var endpoint = builder["ConfigApi"];

        using var httpClient = new HttpClient();
        Policy
            .Handle<Exception>()
            .WaitAndRetry(3, count => TimeSpan.FromSeconds(Math.Pow(2, count)))
            .Execute(() =>
            {
                var response = httpClient.GetAsync(endpoint).Result;
                builder.AddJsonStream(response.Content.ReadAsStream());
            });
    }
}
