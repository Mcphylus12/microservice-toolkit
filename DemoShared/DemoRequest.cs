using Communication.Abstractions.Registration;

namespace DemoShared;

public class DemoRequest : IRequest<DemoResponse>
{
    public DemoRequest(int value)
    {
        Value = value;
    }

    public int Value { get; set; }
}

public class DemoResponse
{
    public DemoResponse()
    {

    }

    public DemoResponse(int value)
    {
        Value = value;
    }

    public int Value { get; set; }
}
