using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerWeb.Services;

namespace ServerWeb.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class WfController : ControllerBase {
    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WfController> _logger;
    private readonly ISingletonSample _singletonSample;
    private readonly ITransientSample _transientSample;

    public WfController(ILogger<WfController> logger,
      ITransientSample transientSample,
      ISingletonSample singletonSample) {
      _logger = logger;
      _transientSample = transientSample;
      _singletonSample = singletonSample;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get() {
      var rng = new Random();
      return Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = rng.Next(-20, 55),
          Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }
  }
}