using Core.Models.Users;
using System.Collections.Generic; // Для IEnumerable
using System; // Для Guid
using Core.Entities; // для UserEntity

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<string> LoginAsync(string email, string password);
        public Task RegisterAsync(string name, string email, string password, string role);
        public Task<UserResponse> GetFromTokenAsync(string token);
        public Task<string> GetIdFromTokenAsync(string token);
        public Task<UserResponse> GetAsync(Guid id); 
        public Task UpdateAsync(Guid id, string name); 
        public Task<string> GetRoleAsync(string token);
        public Task<IEnumerable<UserResponse>> GetAsync(IEnumerable<Guid> ids); 
        public Task DeleteAsync(string token);

        Task<UserEntity?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<UserEntity>> GetAllUsersAsync();
    }
}
