using System;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.EntityTypeConfigurations {
  public abstract class EntityTypeConfiguration<T> : IEntityTypeConfiguration<T> where T : Entity<Guid> {
    public virtual void Configure(EntityTypeBuilder<T> builder) {
      builder.HasKey(b => b.Id);
      builder.Property(b => b.Status).IsRequired().HasMaxLength(10);
      builder.Property(b => b.RowFlag).IsRowVersion().IsConcurrencyToken();
      builder.Property(b => b.Memo).HasMaxLength(200);
    }
  }
}