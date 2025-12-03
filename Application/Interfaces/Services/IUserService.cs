using Core.Models.Response;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        public Task<string> LoginAsync(string email, string password);
        public Task RegisterAsync(string name, string email, string password, string role);
        public Task<UserResponse> GetFromTokenAsync(string token);
        public Task<string> GetIdFromTokenAsync(string token);
        public Task<UserResponse> GetAsync(string id);
        public Task UpdateAsync(string id, string name);
        public Task<string> GetRoleAsync(string token);
        public Task<IEnumerable<UserResponse>> GetAsync(IEnumerable<string> ids);
        public Task DeleteAsync(string token);
    }
}
