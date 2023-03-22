using Microsoft.AspNetCore.Mvc;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastService _service;

    private readonly ILogger<WeatherForecastController> _logger;
    public GetsInfo info { get; set; }

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForecastService service)
    {
        _logger = logger;
        _service = service;
        info = new GetsInfo();
        info.Amount = 5;
        info.Min = -20;
        info.Max = 20;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get([FromBody] GetsInfo info)
    {
        var result = _service.Get(info);
        return result;
    }

    [HttpPost("generate")]
    public ActionResult<IEnumerable<WeatherForecast>> Take([FromQuery] int Amount, [FromBody] MinMax minMax)
    {
        var check = _service.Take(Amount, minMax, info);
        Console.WriteLine();
        if (!check) return BadRequest(Enumerable.Empty<WeatherForecast>());
        var result = _service.Get(info);
        return Ok(result);
    }
}
