using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<UserEntity>
{
    public void Configure(EntityTypeBuilder<UserEntity> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Name).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(255);
        builder.Property(u => u.PasswordHash).IsRequired();
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50);

        // Связь User -> Position
        builder
            .HasOne(u => u.Position)
            .WithMany()
            .HasForeignKey(u => u.PositionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Связь User -> CompletedTasks
        builder
            .HasMany(u => u.CompletedTasks)
            .WithOne()
            .HasForeignKey("CompletedById") // shadow property
            .OnDelete(DeleteBehavior.SetNull);

        // Связь User -> InProcessTasks
        builder
            .HasMany(u => u.InProcessTasks)
            .WithOne(t => t.Receiver)
            .HasForeignKey(t => t.ReceiverId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
