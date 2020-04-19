using Domain.Configs;
using DotNetCore.CAP;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ServerWeb.DomainEventHandlers;

#pragma warning disable 1591

namespace ServerWeb.Services {
  public interface ISingletonSample : IServiceSample {
  }

  public class SingletonSample : ISingletonSample, ICapSubscribe {
    private readonly ILogger<SingletonSample> _logger;
    private readonly IOptionsMonitor<SingletonConfig> _options;

    public SingletonSample(ILogger<SingletonSample> logger, IOptionsMonitor<SingletonConfig> options) {
      _logger = logger;
      _options = options;

      _logger.LogDebug("config setting:{@config}", _options.CurrentValue);
      PrintHastCode("ctor");

      _options.OnChange(op => {
        _logger.LogInformation("SingletonSample Config change:{@newConfig}", _options.CurrentValue);
      });
    }

    public void Dispose() {
      PrintHastCode("Dispose");
    }

    public void PrintHastCode(string ahead) {
      _logger.LogDebug("{@ahead}:{@code}", ahead, GetHashCode());
    }

    /// <summary>
    ///   需要添加<see cref="ICapSubscribe" />接口，否则无法发起订阅。
    /// </summary>
    /// <param name="value"></param>
    /// <param name="header"></param>
    [CapSubscribe("cap-created-entities")]
    public void EntitiesCreatedSubscribe(EntitiesCreatedIntegrationEvent value, [FromCap]CapHeader header) {
      _logger.LogDebug("EntitiesCreatedSubscribe.Headers:{@header} Value{@value}", header, value);
    }
  }

  public static class SingletonSampleExtensions {
    public static IServiceCollection
      AddSingletonSample(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<SingletonConfig>(configuration);
      services.AddSingleton<ISingletonSample, SingletonSample>();
      return services;
    }
  }
}