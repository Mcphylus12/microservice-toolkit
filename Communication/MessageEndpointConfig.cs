namespace Communication;

public class MessageEndpointConfig
{
    public string QueueName { get; set; }
    public string Host { get; set; }
    public string RoutingKey { get; set; }
}
