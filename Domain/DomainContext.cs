using System;
using System.Linq;
using System.Reflection;
using Domain.Entities;
using Domain.EntityTypeConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Domain {
  public class DomainContext : DbContext {
    private readonly ILogger<DomainContext> _logger;

    public DomainContext(ILogger<DomainContext> logger) {
      _logger = logger;
    }

    public DomainContext(ILogger<DomainContext> logger, DbContextOptions<DomainContext> options) : base(options) {
      _logger = logger;
    }

    public DbSet<Company> Companies { get; set; }
    public DbSet<Employee> Employees { get; set; }

    protected override void
      OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      if (optionsBuilder.IsConfigured) {
        base.OnConfiguring(optionsBuilder);
        return;
      }

      optionsBuilder.UseSqlServer(
        "Data Source=(local);Initial Catalog=DomainTest;Integrated Security=False;User ID=sa;Password=estep;Connect Timeout=60;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      var typesToRegister = Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(q =>
          q.GetInterface(typeof(IEntityTypeConfiguration<>).FullName ?? string.Empty) != null);
      foreach (var type in typesToRegister) {
        if (type == typeof(EntityTypeConfiguration<>)) {
          continue;
        }

        dynamic configurationInstance = Activator.CreateInstance(type);
        modelBuilder.ApplyConfiguration(configurationInstance);
      }
    }

    public override void Dispose() {
      _logger.LogDebug("DomainContext dispose.HashCode:{@code}", GetHashCode());
      base.Dispose();
    }
  }
}