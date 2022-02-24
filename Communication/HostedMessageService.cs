using System.Text;
using System.Text.Json;
using Communication.Abstractions;
using Communication.Abstractions.Registration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Abstractions;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Communication;

internal class HostedMessageService : BackgroundService
{
    private readonly IOptions<CommunicationConfiguration> _options;
    private readonly IMonitor<HostedMessageService> _monitor;
    private readonly IResolver _resolver;
    private readonly List<Listener> _listeners;

    public HostedMessageService(
        IOptions<CommunicationConfiguration> options,
        IHostApplicationLifetime applicationLifetime,
        IMonitor<HostedMessageService> monitor,
        IResolver resolver)
    {
        _options = options;
        _monitor = monitor;
        _resolver = resolver;
        _listeners = new List<Listener>();
        applicationLifetime.ApplicationStopping.Register(ApplicationStopping);
    }

    private void ApplicationStopping()
    {
        foreach (var listener in _listeners) listener.Dispose();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_options.Value?.Inbound?.Endpoints is null)
        {
            _monitor.LogWarning("No inbound endpoints were configured so the hosted service is stopping");
            return Task.CompletedTask;
        }

        foreach (var endpointConfig in _options.Value.Inbound.Endpoints)
        {
            _listeners.Add(new Listener(endpointConfig.Key, _resolver, _monitor));
        }
        return Task.CompletedTask;
    }
}

internal sealed class Listener : IDisposable
{
    private IResolver? _resolver;
    private IConnection? _connection;
    private IModel? _channel;
    private readonly IMonitor<HostedMessageService> _monitor;

    public Listener(string endpoint, IResolver resolver, IMonitor<HostedMessageService> monitor)
    {
        Policy
            .Handle<Exception>()
            .WaitAndRetry(3, count => TimeSpan.FromSeconds(Math.Pow(2, count)))
            .Execute(() =>
            {
                monitor.LogInformation("Trying to listen");
                var parts = endpoint.Replace('#', ':').Split('~');
                var host = parts[0];
                var queueName = parts[1];
                var factory = new ConnectionFactory() { HostName = host };
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();
                _channel.QueueDeclare(queue: queueName,
                                durable: false,
                                exclusive: false,
                                autoDelete: false,
                                arguments: null);

                var consumer = new EventingBasicConsumer(_channel);
                consumer.Received += ProcessMessage;
                _channel.BasicConsume(queue: queueName,
                                        autoAck: true,
                                        consumer: consumer);
                _resolver = resolver;
                monitor.LogInformation("Started Listening");
            });
        this._monitor = monitor;
    }

    private void ProcessMessage(object? sender, BasicDeliverEventArgs e)
    {
        try
        {
            _monitor.LogDebug("Message Received");
            var messageType = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["X-Message-Type"]);
            _resolver!.Resolve(messageType, type =>
                {
                    var result = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(e.Body.ToArray()), type);
                    if (result is null) throw new JsonSerializationException();
                    return Task.FromResult(result);
                }
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Dispose()
    {
        _monitor.LogInformation("Stopped Listening");
        _channel?.Dispose();
        _connection?.Dispose();
    }
}
