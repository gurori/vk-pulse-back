// DataAccess/Repositories/RoleRepository.cs
using Application.Interfaces.Repositories;
using Core.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleEntity?> GetByIdAsync(int id)
        {
            return await _context.Roles.FindAsync(id);
        }

        public async Task<IEnumerable<RoleEntity>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }
    }
}
