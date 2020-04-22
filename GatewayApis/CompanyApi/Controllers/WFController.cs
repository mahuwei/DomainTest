using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Models;

namespace CompanyApi.Controllers {
  [ApiController]
  [Route("company/[controller]")]
  public class WFController : ControllerBase {
    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WFController> _logger;

    public WFController(ILogger<WFController> logger) {
      _logger = logger;
    }

    [HttpGet]
    public IEnumerable<WeatherForecast> Get() {
      _logger.LogInformation($"{HttpContext.Request.GetDisplayUrl()}");

      var rng = new Random();
      return Enumerable.Range(1, 5)
        .Select(index => new WeatherForecast {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = rng.Next(-20, 55),
          Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpGet]
    [Route("top")]
    public IEnumerable<WeatherForecast> Get(int count) {
      _logger.LogInformation($"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()}");
      var rng = new Random();
      if (count <= 0) {
        count = 2;
      }

      return Enumerable.Range(count, count)
        .Select(index => new WeatherForecast {
          Date = DateTime.Now.AddDays(index),
          TemperatureC = rng.Next(-20, 55),
          Summary = Summaries[rng.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public ModelData Post([FromBody]ModelData model) {
      _logger.LogInformation($"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()}");
      model.Id++;
      model.Name = $"{model.Name} in company";
      return model;
    }

    [HttpDelete]
    public string Delete([FromBody]ModelData model) {
      _logger.LogInformation($"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()}");
      return$"删除对象【id:{model.Id} name:{model} ";
    }
  }
}