namespace Communication;

#nullable disable
public class InboundMessagingConfig : List<MessageConfig>
{
    public const string Name = "InboundMessages";
}

public class MessageConfig
{
    public string QueueName { get; set; }
    public string Host { get; set; }
    public string RoutingKey { get; set; }
}


public class OutboundMessagingConfig : List<OutboundMessagingConfig.OutboundEndpointConfig>
{
    public const string Name = "OutboundMessages";

    public class OutboundEndpointConfig
    {
        public MessageConfig Endpoint { get; set; }
        public IList<string> Messages { get; set; }
    }
}

public class OutboundRequestConfig : List<OutboundRequestConfig.RequestEndpoint>
{
    public const string Name = "OutboundRequests";

    public class RequestEndpoint
    {
        public string Endpoint { get; set; }
        public IList<string> Requests { get; set; }
    }
}

