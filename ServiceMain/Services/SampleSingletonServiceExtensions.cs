using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceMain.Services {
  public static class SampleSingletonServiceExtensions {
    public static IServiceCollection
      AddSampleSingletonService(this IServiceCollection services, IConfiguration configuration) {
      services.Configure<ConfigSetting>(configuration);
      services.AddSingleton<SampleSingletonService>();
      return services;
    }
  }

  public class ConfigSetting {
    public string Kafka { get; set; }
    public string Mysql { get; set; }
  }
}