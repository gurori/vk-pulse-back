using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public partial class PermissionConfiguration : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.HasKey(p => p.Id);

            var permissions = Enum.GetValues<Permission>()
                .Select(p => new PermissionEntity { Id = (int)p });

            builder.HasData(permissions);
        }
    }
}
