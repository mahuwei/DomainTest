using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceMain.Services {
  public class SampleSingletonService : ISampleService {
    private readonly ILogger<SampleSingletonService> _logger;
    private readonly IOptionsMonitor<ConfigSetting> _options;

    public SampleSingletonService(ILogger<SampleSingletonService> logger, IOptionsMonitor<ConfigSetting> options) {
      _logger = logger;
      _options = options;

      _logger.LogDebug("config setting:{@config}", _options.CurrentValue);

      _options.OnChange(op => {
        _logger.LogInformation("配置更新了，最新的值是:{@newConfig}", _options.CurrentValue);
      });
    }

    public void PrintHastCode() {
      _logger.LogDebug("HashCode:{@code}", GetHashCode());
    }
  }
}