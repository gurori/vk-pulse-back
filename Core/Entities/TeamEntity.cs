// Core/Entities/TeamEntity.cs
using System.Collections.Generic;

namespace Core.Entities
{
    public sealed class TeamEntity : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<UserEntity> Users { get; set; } = new List<UserEntity>();
    }
}
