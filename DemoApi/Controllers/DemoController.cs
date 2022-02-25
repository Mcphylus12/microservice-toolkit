using Communication.Abstractions.Registration;
using DemoShared;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Abstractions;
using System.ComponentModel.DataAnnotations;

namespace DemoApi.Controllers;
[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{
    private readonly ISender _sender;

    public DemoController(
        ISender sender
        )
    {
        this._sender = sender;
    }

    [HttpGet]
    [Route("Request")]
    public async Task<int> SendRequest([FromQuery][Required] int value)
    {
        var response = await _sender.Send(new DemoRequest(value));
        return response.Value;
    }

    [HttpGet]
    [Route("Message")]
    public async Task SendMessage([FromQuery][Required] int value)
    {
        await _sender.Send(new DemoMessage(value));
    }
}
