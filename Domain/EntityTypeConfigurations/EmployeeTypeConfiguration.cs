using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfigurations {
  public class EmployeeTypeConfiguration : EntityTypeConfiguration<Employee> {
    public override void Configure(EntityTypeBuilder<Employee> builder) {
      base.Configure(builder);
      builder.OwnsOne(o => o.Address, a => {
        a.WithOwner();
        a.Property(p => p.City);
        a.Property(p => p.Street);
        a.Property(p => p.ZipCode);
      });
    }
  }
}