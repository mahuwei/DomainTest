using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ServiceMain.Services {
  public interface ISampleService {
    void PrintHastCode();
  }

  public class SampleTransientService : ISampleService {
    private readonly ILogger<SampleTransientService> _logger;

    //private readonly IOptions<TransientServiceConfig> _options;
    private readonly IOptionsSnapshot<TransientServiceConfig> _options;

    public SampleTransientService(ILogger<SampleTransientService> logger,
      IOptionsSnapshot<TransientServiceConfig> options) {
      _logger = logger;
      _options = options;
      _logger.LogDebug("TransientServiceConfig:{@config}", _options.Value);
    }

    public void PrintHastCode() {
      _logger.LogDebug("HashCode:{@code} options:{@options} optionHashCode:{@oh}", GetHashCode(), _options.Value,
        _options.GetHashCode());
    }
  }

  public static class SampleTransientServiceExtensions {
    public static IServiceCollection
      AddSampleTransientService(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<TransientServiceConfig>(configuration);
      //services.AddScoped<ISampleTransientService, SampleTransientService>();
      services.AddTransient<SampleTransientService>();
      return services;
    }
  }

  public class TransientServiceConfig {
    public string Key1 { get; set; }
    public string Key2 { get; set; }
  }
}