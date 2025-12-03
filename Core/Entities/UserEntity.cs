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
        public IEnumerable<TaskEntity> CompletedTasks { get; set; } = [];
        public IEnumerable<TaskEntity> InProcessTasks { get; set; } = [];
    }
}
