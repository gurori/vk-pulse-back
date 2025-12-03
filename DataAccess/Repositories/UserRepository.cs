using Application.Interfaces.Repositories;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<bool> TryCreateAsync(
            string name,
            string email,
            string passwordHash,
            string role
        )
        {
            bool isUserExist = await _context.Users.AsNoTracking().AnyAsync(u => u.Email == email);

            if (isUserExist)
                return false;

            var userEntity = new UserEntity()
            {
                Role = role,
                Email = email,
                Name = name,
                PasswordHash = passwordHash,
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context
                .Users.AsNoTracking()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<UserEntity?> GetByIdAsync(string id)
        {
            return await _context.Users.AsNoTracking().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetManyByIdAsync(IEnumerable<string> ids)
        {
            var userEntities = await _context
                .Users.AsNoTracking()
                .Where(u => ids.Contains(u.Id))
                .ToListAsync();

            return userEntities;
        }

        public async Task<string?> GetRoleByIdAsync(string id)
        {
            return await _context
                .Users.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => u.Role)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(string id, string name)
        {
            await _context
                .Users.Where(u => u.Id == id)
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(u => u.Name, u => name)
                // .SetProperty(u => u.FirstName, u => firstName)
                // .SetProperty(u => u.MiddleName, u => middleName)
                // .SetProperty(u => u.Description, u => description)
                // .SetProperty(u => u.JobTitle, u => jobTitle)
                );

            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(string id)
        {
            await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync();
        }
    }
}
