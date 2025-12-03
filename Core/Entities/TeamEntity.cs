namespace Core.Entities
{
    public sealed class TeamEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public IEnumerable<UserEntity> Users { get; set; } = [];
        public IEnumerable<TaskEntity> Tasks { get; set; } = [];

        // Если успеем
        public string? AdminId = null;
    }
}
