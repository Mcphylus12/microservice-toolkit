using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Monitoring.Abstractions;

namespace DemoConfigurationApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IMonitor<ConfigController> _monitor;

    public ConfigController(IMonitor<ConfigController> monitor)
    {
        _monitor = monitor;
    }

    [HttpGet(Name = "GetConfig")]
    public object Get([FromQuery][Required] string service)
    {
        var result = new Dictionary<string, dynamic>();
        IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("config.json");
        configurationBuilder.AddJsonFile($"config.{service}.json", true);

        return BuildValue(configurationBuilder.Build());
    }

    private object BuildValue(IConfiguration config)
    {
        var items = config.GetChildren();

        {
            var result = new List<object>();
            if (items.First().Key == "0")
            {
                foreach (var item in items)
                {
                    result.Add(BuildValueFromSection(item));
                }

                return result;
            }
        }

        {
            var result = new Dictionary<string, object>();
            foreach (var item in items)
            {
                result[item.Key] = BuildValueFromSection(item);
            }

            return result;
        }
    }

    private object BuildValueFromSection(IConfigurationSection config)
    {
        if (config.Value is not null) return config.Value;

        return BuildValue(config);
    }
}
