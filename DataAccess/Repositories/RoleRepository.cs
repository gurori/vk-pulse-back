using Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public sealed class RoleRepository(AppDbContext context) : IRoleRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<HashSet<int>> GetPermissionsIdsAsync(string roleName)
        {
            var permissions = await _context
                .Roles.Include(r => r.Permissions)
                .Where(r => r.Name.ToLower() == roleName.ToLower())
                .Select(r => r.Permissions)
                .ToArrayAsync();

            return permissions.SelectMany(p => p).Select(p => p.Id).ToHashSet();
        }
    }
}
