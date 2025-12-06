namespace Core.Entities
{
    public sealed class RolePermissionEntity
    {
        public int RoleId { get; set; }
        public RoleEntity Role { get; set; } = default!;

        public int PermissionId { get; set; }
        public PermissionEntity Permission { get; set; } = default!;
    }
}
