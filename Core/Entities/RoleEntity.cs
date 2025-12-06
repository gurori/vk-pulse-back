using System.Collections.Generic;

namespace Core.Entities
{
    public sealed class RoleEntity : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<RolePermissionEntity> Permissions { get; set; } = new List<RolePermissionEntity>();
    }
}
