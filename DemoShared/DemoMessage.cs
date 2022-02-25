using Communication.Abstractions.Registration;

namespace DemoShared;

public class DemoMessage : IMessage
{
    public DemoMessage()
    {

    }

    public DemoMessage(int value)
    {
        Value = value;
    }

    public string MessageType => "DemoMessage";

    public int Value { get; set; }
}
