using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; 
using Core.Entities; 
using DataAccess.Data;

namespace DataAccess.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEntity>> GetAllAsync();
        Task<UserEntity?> GetByEmailAsync(string email);
        Task<UserEntity?> GetByIdAsync(Guid id); 
        Task<IEnumerable<UserEntity>> GetManyByIdAsync(IEnumerable<Guid> ids); 
        Task<string?> GetRoleByIdAsync(Guid id); 
        Task UpdateAsync(Guid id, string name);
        Task DeleteByIdAsync(Guid id); 
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context = context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            return await _context.Users
                                 .Include(u => u.Team) // Включаем связанную команду
                                 .Include(u => u.Position) // Включаем связанную позицию
                                 .Include(u => u.CompletedTasks) // Включаем завершенные задачи
                                 .Include(u => u.InProcessTasks) // Включаем задачи в процессе
                                 .AsNoTracking()
                                 .ToListAsync();
        }

        public async Task<UserEntity?> GetByEmailAsync(string email)
        {
            return await _context
                .Users
                .Include(u => u.Team)
                .Include(u => u.Position)
                .Include(u => u.CompletedTasks) 
                .Include(u => u.InProcessTasks) 
                .AsNoTracking()
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }

        public async Task<UserEntity?> GetByIdAsync(Guid id) 
        {
            return await _context.Users
                                 .Include(u => u.Team) 
                                 .Include(u => u.Position) 
                                 .Include(u => u.CompletedTasks) 
                                 .Include(u => u.InProcessTasks) 
                                 .AsNoTracking()
                                 .FirstOrDefaultAsync(u => u.Id == id); // Используем Guid для сравнения
        }

        public async Task<IEnumerable<UserEntity>> GetManyByIdAsync(IEnumerable<Guid> ids)
        {
            var userEntities = await _context
                .Users
                .Include(u => u.Team) 
                .Include(u => u.Position) 
                .Include(u => u.CompletedTasks) 
                .Include(u => u.InProcessTasks) 
                .AsNoTracking()
                .Where(u => ids.Contains(u.Id)) // Используем Guid для сравнения
                .ToListAsync();

            return userEntities;
        }

        public async Task<string?> GetRoleByIdAsync(Guid id) 
        {
            return await _context
                .Users.AsNoTracking()
                .Where(u => u.Id == id) // Используем Guid для сравнения
                .Select(u => u.Role)
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Guid id, string name)
        {
            await _context
                .Users.Where(u => u.Id == id) // Используем Guid для сравнения
                .ExecuteUpdateAsync(s =>
                    s.SetProperty(u => u.Name, u => name)
                );

            await _context.SaveChangesAsync(); 
        }

        public async Task DeleteByIdAsync(Guid id) 
        {
            await _context.Users.Where(x => x.Id == id).ExecuteDeleteAsync(); // Используем Guid для сравнения
        }
    }
}
