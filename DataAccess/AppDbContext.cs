using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DataAccess
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options,
        IOptions<AuthorizationOptions> authOptions
    ) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<RolePermissionEntity>().HasData(ParseRolePermissions());
        }

        private RolePermissionEntity[] ParseRolePermissions() =>
            authOptions
                .Value.RolePermissions.SelectMany(rp =>
                    rp.Permissions.Select(p => new RolePermissionEntity
                    {
                        RoleId = (int)Enum.Parse<Role>(rp.Role),
                        PermissionId = (int)Enum.Parse<Permission>(p),
                    })
                )
                .ToArray();
    }
}
