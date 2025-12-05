using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public sealed class TeamConfiguration : IEntityTypeConfiguration<TeamEntity>
{
    public void Configure(EntityTypeBuilder<TeamEntity> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(255);

        // Связь Team -> Users
        builder
            .HasMany(t => t.Users)
            .WithOne(u => u.Team)
            .HasForeignKey(u => u.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // Связь Team -> Tasks
        builder
            .HasMany(t => t.Tasks)
            .WithOne()
            .HasForeignKey("TeamId") // shadow property, если нужно
            .OnDelete(DeleteBehavior.SetNull);
    }
}
