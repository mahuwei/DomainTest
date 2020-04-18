using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServerWeb.Commands.Wf.Publish;
using ServerWeb.Commands.Wf.Send;
using ServerWeb.Services;

namespace ServerWeb.Controllers {
  /// <summary>
  ///   测试Controller
  /// </summary>
  [ApiExplorerSettings(GroupName = "v1")]
  [ApiController]
  [Route("[controller]")]
  public class WfController : ControllerBase {
    private static readonly string[] Summaries = {
      "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WfController> _logger;
    private readonly IMediator _mediator;
    private readonly ISingletonSample _singletonSample;
    private readonly ITransientSample _transientSample;

    /// <summary>
    ///   构造函数
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="transientSample"></param>
    /// <param name="singletonSample"></param>
    /// <param name="mediator"></param>
    public WfController(ILogger<WfController> logger,
      ITransientSample transientSample,
      ISingletonSample singletonSample,
      IMediator mediator) {
      _logger = logger;
      _transientSample = transientSample;
      _singletonSample = singletonSample;
      _mediator = mediator;
    }

    /// <summary>
    ///   获取天气预报:单播消息传输
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    ///   IMediator.Send
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("send")]
    public async Task<Employee> Send() {
      _logger.LogDebug("start Send()...");
      var ret = await _mediator.Send(new GetSend());
      _logger.LogDebug("Send() result:{@ret}", ret);
      return ret;
    }

    /// <summary>
    ///   IMediator.Publish:多播消息传输
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task Publish(string info) {
      _logger.LogDebug("start Publish({@info})...", info);
      await _mediator.Publish(new Ping { Info = info });
      _logger.LogDebug("Publish() end.");
    }
  }
}