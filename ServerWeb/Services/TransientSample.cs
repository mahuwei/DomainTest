using Domain.Configs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServerWeb.Services {
  public interface ITransientSample : IServiceSample {
  }

  public class TransientSample : ITransientSample {
    private readonly ILogger<TransientSample> _logger;
    private readonly IOptionsSnapshot<TransientConfig> _options;

    public TransientSample(ILogger<TransientSample> logger, IOptionsSnapshot<TransientConfig> options) {
      _logger = logger;
      _options = options;
      PrintHastCode("ctor");
      _logger.LogDebug("TransientSample options:{@options}", options);
    }

    public void Dispose() {
      PrintHastCode("Dispose");
    }

    public void PrintHastCode(string ahead) {
      _logger.LogDebug("{@ahead}:{@code}", ahead, GetHashCode());
    }
  }

  public static class TransientSampleExtensions {
    public static IServiceCollection
      AddTransientSample(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<TransientConfig>(configuration);
      services.AddTransient<ITransientSample, TransientSample>();
      return services;
    }
  }
}