using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;

namespace BusinessApi {
  public class TestService : IHostedService {
    private readonly ILogger<TestService> _logger;
    private BaseConfig _config;

    public TestService(ILogger<TestService> logger, IOptionsMonitor<BaseConfig> options) {
      _logger = logger;

      _logger.LogDebug("config setting:{@config}",
        JsonConvert.SerializeObject(options.CurrentValue, Formatting.Indented));
      _config = options.CurrentValue;

      options.OnChange(op => {
        _logger.LogInformation("HostSample Config change:{@newConfig}",
          JsonConvert.SerializeObject(options.CurrentValue, Formatting.Indented));

        if (_config.Kafka != options.CurrentValue.Kafka) {
          _logger.LogInformation($"Kafka发生变化:{_config.Kafka} ==> {options.CurrentValue.Kafka}");
        }
        else {
          _logger.LogInformation($"Kafka没有发生变化:{_config.Kafka} ==> {options.CurrentValue.Kafka}");
        }

        _config = options.CurrentValue;
      });
    }

    public Task StartAsync(CancellationToken cancellationToken) {
      _logger.LogDebug("StartAsync...");
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) {
      _logger.LogDebug("StopAsync...");
      return Task.CompletedTask;
    }
  }
}