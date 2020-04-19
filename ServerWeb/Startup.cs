using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Domain;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ServerWeb.Services;

#pragma warning disable 1591

namespace ServerWeb {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services) {
      services.AddControllers();

      services.AddMediatR(typeof(Program).Assembly);
      var assemblies = typeof(Entity<>).Assembly;
      services.AddMediatR(assemblies);
      services.AddAutoMapper(assemblies);

      services.AddTransientSample(Configuration.GetSection("TransientConfig"));
      services.AddSingletonSample(Configuration.GetSection("SingletonConfig"));
      services.AddHostSample(Configuration.GetSection("HostConfig"));

      services.AddSwaggerGen(c => {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ServerWeb V1", Version = "v1" });
        c.SwaggerDoc("v2", new OpenApiInfo { Title = "ServerWeb V2", Version = "v2" });

        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
      });

      services.AddDbContext<DomainContext>(builder => {
        builder.UseSqlServer(Configuration.GetConnectionString("DBConnectString"));
      });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DomainContext dc) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      dc.Database.Migrate();
      SeedData.Create(dc);
      dc.Dispose();

      app.UseSwagger();
      app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ServerWeb V1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "ServerWeb V2");
      });

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });
    }
  }
}