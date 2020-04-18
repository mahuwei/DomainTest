using System.Threading;
using System.Threading.Tasks;
using Domain.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServerWeb.Services {
  public interface IHostSample : IServiceSample, IHostedService {
  }

  public class HostSample : IHostSample {
    private readonly ILogger<HostSample> _logger;
    private readonly IOptionsMonitor<HostConfig> _options;

    public HostSample(ILogger<HostSample> logger, IOptionsMonitor<HostConfig> options) {
      _logger = logger;
      _options = options;

      _logger.LogDebug("config setting:{@config}", _options.CurrentValue);
      PrintHastCode("ctor");

      _options.OnChange(op => {
        _logger.LogInformation("HostSample Config change:{@newConfig}", _options.CurrentValue);
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

    public void Dispose() {
      PrintHastCode("Dispose");
    }

    public void PrintHastCode(string ahead) {
      _logger.LogDebug("{@ahead}:{@code}", ahead, GetHashCode());
    }
  }

  public static class HostSampleExtensions {
    public static IServiceCollection AddHostSample(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<HostConfig>(configuration);
      services.AddHostedService<HostSample>();
      return services;
    }
  }
}