using MiApiConSerilog.Models;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace MiApiConSerilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            Log.Information("Solicitud GET procesada");
	    Log.Warning("Mapeo de log warning");
	    Log.Error("Mapeo de log con error");
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody] WeatherForecast forecast)
        {
            Log.Information("Solicitud POST recibida con los siguientes datos: {Forecast}", forecast);
            return Ok(forecast);
        }
    }
}
