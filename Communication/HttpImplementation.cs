using System.Text;
using Communication.Abstractions.Registration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Communication;

internal class HttpImplementation : IRequestImplementation
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly Dictionary<string, string> _options;

    public HttpImplementation(
        IHttpClientFactory httpClientFactory,
        IOptions<OutboundRequestConfig> options)
    {
        _httpClientFactory = httpClientFactory;
        _options = new Dictionary<string, string>();

        foreach (var endpoint in options.Value)
        {
            foreach (var request in endpoint.Requests)
            {
                _options.Add(request, endpoint.Endpoint);
            }
        }
    }

    public async Task<TResponse> SendRequest<TResponse>(IRequest<TResponse> request)
    {
        var response = await PostRequest(request);

        try
        {
            var nResult = JsonConvert.DeserializeObject<TResponse>(await response!.Content.ReadAsStringAsync());
            if (nResult is null) throw new JsonSerializationException("Deserialisation succeeded but result was null");
            return nResult;
        }
        catch (Exception ex)
        {
            throw new JsonSerializationException("Error Deserialising request", ex);
        }
    }

    public Task SendRequest(IRequest request) => PostRequest(request);

    private async Task<HttpResponseMessage> PostRequest(IRequest request)
    {
        try
        {
            var endpoint = _options[request.GetType().Name];
            var client = _httpClientFactory.CreateClient();
            var content = JsonConvert.SerializeObject(request);
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            httpRequest.Headers.Add("X-Request-Type", request.GetType().Name);

            var response = await client.SendAsync(httpRequest);
            response.EnsureSuccessStatusCode();
            return response;
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Error Sending request", ex);
        }
    }
}
