using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Models.CustomException;

namespace BusinessApi.Controllers {
  [ApiController]
  [Route("[controller]")]
  public class WFController : ControllerBase {
    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static int _runCount;
    private readonly IConfiguration _configuration;
    private readonly IConsulClient _consulClient;

    private readonly ILogger<WFController> _logger;
    private readonly IOptions<BaseConfig> _options;
    private readonly IOptionsSnapshot<BaseConfig> _optionsSnapshot;

    public WFController(ILogger<WFController> logger,
      IConsulClient consulClient,
      IConfiguration configuration,
      IOptions<BaseConfig> options,
      IOptionsSnapshot<BaseConfig> optionsSnapshot) {
      _logger = logger;
      _consulClient = consulClient;
      _configuration = configuration;
      _options = options;
      _optionsSnapshot = optionsSnapshot;
    }

    [HttpGet]
    [Route("config")]
    public ActionResult<IEnumerable<string>> GetConfig() {
      // _options.Value.Mysql 配置更新后，不会更新
      return new[] { _configuration["BaseConfig:Kafka"], _options.Value.Mysql, _optionsSnapshot.Value.Redis };
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
    [Route("kv")]
    public async Task<ActionResult<string>> GetKv(string key) {
      var res = await _consulClient.KV.Get(key);
      if (res.StatusCode != HttpStatusCode.OK) {
        return BadRequest($"Error:{res.StatusCode}");
      }

      var str = Encoding.UTF8.GetString(res.Response.Value);
      return Ok(str);
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
    public ModelData Post([FromBody]ModelData model, [FromHeader]string myReq) {
      _runCount++;
      _logger.LogInformation($"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()} myReq:{myReq}");
      var mod = _runCount % 4;
      switch (mod) {
        case 1:
          throw new FirstException($"Company firstError:{_runCount}");
        case 2:
          throw new SecondException($"Company secondError:{_runCount}");
        case 3:
          throw new Exception($"Company secondError:{_runCount}");
      }

      model.Id++;
      model.Name = $"{model.Name} in business";
      return model;
    }

    [HttpDelete]
    public string Delete([FromBody]ModelData model) {
      _logger.LogInformation($"{HttpContext.Request.Method} {HttpContext.Request.GetDisplayUrl()}");
      return $"删除对象【id:{model.Id} name:{model} ";
    }
  }
}