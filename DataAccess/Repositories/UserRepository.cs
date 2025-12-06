// DataAccess/Repositories/UserRepository.cs
using Application.Interfaces.Repositories;
using Core.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Ulid;
using System.Collections.Generic;

namespace DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> TryCreateAsync(string name, string email, string passwordHash, string role)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Email == email);
            if (existingUser)
            {
                return false;
            }

            var user = new UserEntity
            {
                Id = Ulid.NewUlid().ToString(),
                Name = name,
                Email = email,
                PasswordHash = passwordHash,
                Role = role,
                Score = 0
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> GetByIdAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Include(u => u.Position)
                .Include(u => u.TasksReceived)
                    .ThenInclude(t => t.Receiver)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(string id, string name)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Name = name;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string?> GetRoleByIdAsync(string id)
        {
            return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetManyByIdAsync(IEnumerable<string> ids)
        {
            return await _context.Users
                .Include(u => u.Team)
                .Include(u => u.Position)
                .Include(u => u.TasksReceived)
                    .ThenInclude(t => t.Receiver)
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            if (userToDelete != null)
            {
                _context.Users.Remove(userToDelete);
                await _context.SaveChangesAsync();
            }
        }
    }
}
