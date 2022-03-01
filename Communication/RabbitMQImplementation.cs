using Communication.Abstractions.Registration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Communication;

internal class RabbitMQImplementation : IMessageImplementation
{
    private readonly Dictionary<string, MessageEndpointConfig> _options;

    public RabbitMQImplementation(IOptions<OutboundMessagingConfig> options)
    {
        _options = new Dictionary<string, MessageEndpointConfig>();

        foreach (var endpoint in options.Value)
        {
            foreach (var message in endpoint.Messages)
            {
                _options.Add(message, endpoint.Endpoint);
            }
        }
    }

    public Task SendMessage(IMessage message)
    {
        var endpoint = _options[message.GetType().Name];
        var host = endpoint.Host;
        var queueName = endpoint.QueueName;
        var routingKey = endpoint.RoutingKey;
        var factory = new ConnectionFactory() { HostName = host };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: queueName,
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
            var headers = channel.CreateBasicProperties();
            headers.Headers = new Dictionary<string, object>
            {
                ["X-Message-Type"] = message.GetType().Name
            };

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));

            channel.BasicPublish(exchange: "",
                                 routingKey: routingKey,
                                 basicProperties: headers,
                                 body: body);
        }

        return Task.CompletedTask;
    }
}
