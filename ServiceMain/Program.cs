using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Serilog;
using ServiceMain.Services;

namespace ServiceMain {
  class Program {
    public static IConfiguration Configuration { get; } =
      new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
          true)
        .AddJsonFile("serilog.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    static async Task Main(string[] args) {
      var serviceCollection = new ServiceCollection();
      // 用工厂模式将配置对象注册到容器管理
      serviceCollection.AddSingleton(p => Configuration);
      serviceCollection.AddLogging(builder => {
        builder.AddSerilog(new LoggerConfiguration().ReadFrom.Configuration(Configuration)
          .Enrich.FromLogContext()
          .CreateLogger(), true);
      });

      serviceCollection.AddMediatR(typeof(Program).Assembly);
      serviceCollection.AddMediatR(typeof(Entity<>).Assembly);

      serviceCollection.AddSampleSingletonService(Configuration.GetSection("ConfigSetting"));
      // 可以替换为下面两条语句，具有相同的效果
      //serviceCollection.Configure<ConfigSetting>(Configuration.GetSection("ConfigSetting"));
      //serviceCollection.AddSingleton<SampleSingletonService>();

      serviceCollection.Configure<TransientServiceConfig>(Configuration.GetSection("TransientServiceConfig"));
      serviceCollection.AddTransient<SampleTransientService>();
      //serviceCollection.AddSampleTransientService(Configuration.GetSection("TransientServiceConfig"));

      var service = serviceCollection.BuildServiceProvider();

      // 对应强类型配置，直接通过： Configuration.GetSection("ConfigSetting")返回value = null;
      //var configSetting = Configuration.GetSection("ConfigSetting").Get<ConfigSetting>();

      var logger = service.GetService<ILogger<Program>>();

      ChangeToken.OnChange(() => Configuration.GetReloadToken(), () => {
        logger.LogDebug("config change:{@config}", Configuration.GetSection("ConfigSetting").Get<ConfigSetting>());
      });

      // serilog 不支持 IncludeScopes
      //while (Console.ReadKey().Key != ConsoleKey.Escape) {
      //  using (logger.BeginScope("ScopeId:{scopeId}", Guid.NewGuid())) {
      //    logger.LogDebug("这是Debug");
      //    logger.LogInformation("这是Info");
      //    logger.LogWarning("这是Warning");
      //    logger.LogError("这是Error");
      //  }

      //  Thread.Sleep(100);
      //  Console.WriteLine("============分割线=============\n");
      //}
      var helps = new List<string> { "help:获取帮助信息", "test-config:测试配置；可以手工修改配置（appsettings.json)查看", "quit:退出" };
      var isStop = false;
      PrintHelps(helps);
      do {
        var input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) {
          continue;
        }

        var commands = input.Split(' ');
        switch (commands[0]) {
          case"help":
            PrintHelps(helps);
            break;
          case"test-config":
            await TestConfig(service, logger);
            break;
          case"quit":
            isStop = true;
            break;
        }

        if (isStop) {
          break;
        }
      } while (true);

      Console.WriteLine("按任意键退出...");
      Console.ReadKey();
    }

    private static void PrintHelps(List<string> helps) {
      Console.WriteLine("帮助信息:");
      foreach (var help in helps) {
        var texts = help.Split(":");
        Console.WriteLine("  {0,-15} :{1}", texts[0], texts[1]);
      }

      Console.WriteLine("\n");
    }

    private static async Task TestConfig(ServiceProvider service, ILogger<Program> logger) {
      var employee = new Employee { Name = "马虎维" };
      var mediator = service.GetService<IMediator>();
      var ret = await mediator.Send(employee);
      logger.LogDebug("add employee result:{@ret}", ret);
    }
  }
}