using System.Collections.Generic;

namespace Core.Entities
{
    public sealed class PermissionEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RolePermissionEntity> Roles { get; set; } = new List<RolePermissionEntity>();
    }
}
