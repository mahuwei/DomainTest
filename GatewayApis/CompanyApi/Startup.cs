using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models;
using Models.Filters;
using Newtonsoft.Json;

namespace CompanyApi {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers(options => { options.Filters.Add<CustomExceptionFilterAttribute>(); });
      services.AddHealthChecks();
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