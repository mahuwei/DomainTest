using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Winton.Extensions.Configuration.Consul;

namespace BusinessApi {
  public class Program {
    public static IConfiguration Configuration { get; } =
      new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
          true)
        .AddJsonFile("consul.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    public static void Main(string[] args) {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
      return Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) => {
          config.AddConsul("ServerConfig", options => {
            // 这一句没有运行也正常！可能是默认的
            //options.Parser = new SimpleConfigurationParser();
            options.ConsulConfigurationOptions = configuration => {
              options.ConsulConfigurationOptions =
                cco => {
                  //cco.Address = new Uri("http://192.168.1.129:8500");
                  cco.Address = new Uri($"{Configuration["Consul:ServicesIp"]}:{Configuration["Consul:Port"]}");
                };
              options.Optional = true;
              options.ReloadOnChange = true;
              options.OnLoadException = exceptionContext => {
                Console.WriteLine("error:" + exceptionContext.Exception.Message);
                exceptionContext.Ignore = true;
              };
            };
          });

          config
            .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
            .AddJsonFile("consul.json", false, true)
            .AddEnvironmentVariables()
            .AddCommandLine(args);
        })
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder.UseStartup<Startup>();
        });
    }
  }
}