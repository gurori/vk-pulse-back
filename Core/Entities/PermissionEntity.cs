namespace Core.Entities
{
    public sealed class PermissionEntity
    {
        public int Id { get; set; }
        public ICollection<RoleEntity> Roles { get; set; } = [];
    }
}
