namespace Core.Entities
{
    public sealed class TeamEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<UserEntity> Users { get; set; } = [];
        public ICollection<TaskEntity> Tasks { get; set; } = [];

        // Если успеем
        public string? AdminId = null;
    }
}
