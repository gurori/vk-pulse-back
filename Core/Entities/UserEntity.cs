// Core/Entities/UserEntity.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Core.Entities
{
    public sealed class UserEntity : BaseEntity
    {
        public string Role { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public int Score { get; set; } = 0;

        public string? TeamId { get; set; } = null;
        public TeamEntity? Team { get; set; } = null;

        public string? PositionId { get; set; } = null;
        public PositionEntity? Position { get; set; } = null;

        public ICollection<TaskEntity> TasksReceived { get; set; } = new List<TaskEntity>();

        [NotMapped]
        public ICollection<TaskEntity> InProcessTasks => TasksReceived.Where(t => !t.IsCompleted).ToList();

        [NotMapped]
        public ICollection<TaskEntity> CompletedTasks => TasksReceived.Where(t => t.IsCompleted).ToList();
    }
}
