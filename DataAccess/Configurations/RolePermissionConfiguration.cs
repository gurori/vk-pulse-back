using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public partial class RolePermissionConfiguration
        : IEntityTypeConfiguration<RolePermissionEntity>
    {
        public void Configure(EntityTypeBuilder<RolePermissionEntity> builder)
        {
            builder.HasKey(r => new { r.RoleId, r.PermissionId });
        }
    }
}
