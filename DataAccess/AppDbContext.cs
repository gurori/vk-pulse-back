// DataAccess/AppDbContext.cs
using Core.Entities;
using Core.Enums;
using Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace DataAccess
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options,
        IOptions<AuthorizationOptions> authOptions
    ) : DbContext(options)
    {
        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<RoleEntity> Roles { get; set; } = default!;
        public DbSet<PermissionEntity> Permissions { get; set; } = default!;
        public DbSet<RolePermissionEntity> RolePermissions { get; set; } = default!;
        public DbSet<TaskEntity> Tasks { get; set; } = default!;
        public DbSet<TeamEntity> Teams { get; set; } = default!;
        public DbSet<PositionEntity> Positions { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // Seed RoleEntity data
            modelBuilder.Entity<RoleEntity>().HasData(
                Enum.GetValues(typeof(Role))
                    .Cast<Role>()
                    .Select(r => new RoleEntity { Id = (int)r, Name = r.ToString() })
            );

            // Seed PermissionEntity data
            modelBuilder.Entity<PermissionEntity>().HasData(
                Enum.GetValues(typeof(Permission))
                    .Cast<Permission>()
                    .Select(p => new PermissionEntity { Id = (int)p, Name = p.ToString() })
            );

            // Seed RolePermissionEntity data based on AuthorizationOptions
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
