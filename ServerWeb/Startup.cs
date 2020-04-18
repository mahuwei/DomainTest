using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServerWeb.Services;

namespace ServerWeb {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();

      services.AddMediatR(typeof(Program).Assembly);
      services.AddMediatR(typeof(Entity<>).Assembly);

      services.AddTransientSample(Configuration.GetSection("TransientConfig"));
      services.AddSingletonSample(Configuration.GetSection("SingletonConfig"));
      services.AddHostSample(Configuration.GetSection("HostConfig"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}