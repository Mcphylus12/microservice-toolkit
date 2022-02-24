using Communication.Abstractions;
using Communication.Abstractions.Registration;
using Microsoft.Extensions.Logging;
using Monitoring.Abstractions;
using Newtonsoft.Json;
using System.Text;

namespace Communication;

internal class HttpImplementation : IRequestImplementation
{
    private readonly IHttpClientFactory _httpClientFactory;

    public HttpImplementation(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<TResponse> SendRequest<TResponse>(string endpoint, IRequest<TResponse> request)
    {
        var response = await PostRequest(endpoint, request);

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

    public Task SendRequest(string endpoint, IRequest request) => PostRequest(endpoint, request);

    private async Task<HttpResponseMessage> PostRequest(string endpoint, IRequest request)
    {
        try
        {
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
