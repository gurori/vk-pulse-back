// DataAccess/Configurations/TaskEntityConfiguration.cs
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TaskEntityConfiguration : IEntityTypeConfiguration<TaskEntity>
    {
        public void Configure(EntityTypeBuilder<TaskEntity> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Id).ValueGeneratedOnAdd(); // Ulid генерируется приложением

            builder.Property(t => t.Name).IsRequired().HasMaxLength(255);
            builder.Property(t => t.Description).HasMaxLength(1000);
            builder.Property(t => t.Score).IsRequired();
            builder.Property(t => t.StartDate).IsRequired();
            builder.Property(t => t.EndDate).IsRequired();
            builder.Property(t => t.IsCompleted).IsRequired();

            builder.HasOne(t => t.Receiver)
                   .WithMany(u => u.TasksReceived)
                   .HasForeignKey(t => t.ReceiverId)
                   .OnDelete(DeleteBehavior.SetNull); // При удалении пользователя, ReceiverId задач станет NULL
        }
    }
}
