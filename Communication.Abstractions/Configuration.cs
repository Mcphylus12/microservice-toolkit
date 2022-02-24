namespace Communication.Abstractions;

public class CommunicationConfiguration
{
    public const string Name = "Communication";
    public OutboundConfiguration? Outbound { get; set; }
    public MessagingConfiguration? Inbound { get; set; }
}

public class OutboundConfiguration
{
    public MessagingConfiguration? Messaging { get; set; }
    public RequestsOutConfiguration? Requests { get; set; }
}

public class RequestsOutConfiguration
{
    public Dictionary<string, List<string>>? Endpoints { get; set; }
}

public class MessagingConfiguration
{
    public Dictionary<string, List<string>>? Endpoints { get; set; }
}
