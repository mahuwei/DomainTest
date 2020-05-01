using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Models.CustomException;
using Models.Filters;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

namespace GateWayServer {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers(options => { options.Filters.Add<CustomExceptionFilterAttribute>(); });
      services.AddOcelot(Configuration).AddConsul();
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
      });

      app.UseOcelot(configuration => {
          configuration.PreErrorResponderMiddleware = async (ctx, next) => {
            try {
              ctx.HttpContext.Request.Headers.Add("myreq", $"ocelot-request:{Guid.NewGuid()}");
              await next.Invoke();
            }
            catch (FirstException fe) {
              Console.WriteLine(fe.Message);
              throw;
            }
            catch (SecondException se) {
              Console.WriteLine(se.Message);
              throw;
            }
            catch (Exception ex) {
              Console.WriteLine(ex.Message);
              throw;
            }
          };
        })
        .Wait();
    }
  }
}