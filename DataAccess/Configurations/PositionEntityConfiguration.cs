// DataAccess/Configurations/PositionEntityConfiguration.cs
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class PositionEntityConfiguration : IEntityTypeConfiguration<PositionEntity>
    {
        public void Configure(EntityTypeBuilder<PositionEntity> builder)
        {
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd(); // Предполагаем Ulid
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        }
    }
}
