using System;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Filters;
using Newtonsoft.Json;

namespace BusinessApi {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers(options => { options.Filters.Add<CustomExceptionFilterAttribute>(); });
      services.AddHealthChecks();

      services.AddSingleton<IConsulClient, ConsulClient>(p => new ConsulClient(consulConfig => {
        //consul address  
        consulConfig.Address = new Uri($"{Configuration["Consul:ServicesIp"]}:{Configuration["Consul:Port"]}");
      }, null, handlerOverride => {
        //disable proxy of httpClient handler  
        handlerOverride.Proxy = null;
        handlerOverride.UseProxy = false;
      }));

      services.Configure<BaseConfig>(Configuration.GetSection("BaseConfig"));
      services.AddHostedService<TestService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
        endpoints.MapHealthChecks("/health");
      });

      var ret = ConsulHelper.ServiceRegister(Configuration).Result;
      Console.WriteLine(JsonConvert.SerializeObject(ret, Formatting.Indented));
    }
  }
}