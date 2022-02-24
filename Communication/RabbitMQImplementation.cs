using Communication.Abstractions;
using Communication.Abstractions.Registration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace Communication;

internal class RabbitMQImplementation : IMessageImplementation
{
    public Task SendMessage(string endpoint, IMessage message)
    {
        var parts = endpoint.Split('~');
        var host = parts[0];
        var queueName = parts[1];
        var routingKey = parts[2];
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
