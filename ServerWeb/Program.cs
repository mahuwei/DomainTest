using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

#pragma warning disable 1591

namespace ServerWeb {
  public class Program {
    public static IConfiguration Configuration { get; } =
      new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false, true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
          true)
        .AddJsonFile("serilog.json", false, true)
        .AddEnvironmentVariables()
        .Build();

    public static void Main(string[] args) {
      //CreateHostBuilder(args).Build().Run();
      Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(Configuration)
        .CreateLogger();

      Log.Information("App start...");
      Console.Title = @"Domain Web Server";

      try {
        CreateHostBuilder(args).Build().Run();
        Log.Information("程序结束。");
      }
      catch (Exception ex) {
        Log.Fatal(ex, "意外终止。");
      }
      finally {
        Log.Information("Domain Web Server quit.");
        Log.CloseAndFlush();
      }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) {
      return Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder.UseStartup<Startup>();
        });
    }
  }
}