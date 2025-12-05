using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public sealed class PositionConfiguration : IEntityTypeConfiguration<PositionEntity>
{
    public void Configure(EntityTypeBuilder<PositionEntity> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
        builder.Property(p => p.Description).HasMaxLength(1000);
        builder.Property(p => p.Salary);
    }
}