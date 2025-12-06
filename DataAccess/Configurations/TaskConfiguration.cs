using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public sealed class TaskConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name).IsRequired().HasMaxLength(255);
            builder.Property(t => t.Description).HasMaxLength(1000);

            builder
                .HasOne(t => t.Creator)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(t => t.CreatorId)
                .OnDelete(DeleteBehavior.SetNull);

            builder
                .HasOne(t => t.Receiver)
                .WithMany(u => u.ReceivedTasks)
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
