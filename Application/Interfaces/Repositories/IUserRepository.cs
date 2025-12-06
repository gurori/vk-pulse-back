// Application/Interfaces/Repositories/IUserRepository.cs
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> TryCreateAsync(
            string name,
            string email,
            string passwordHash,
            string role
        );

        public Task<UserEntity?> GetByEmailAsync(string email);

        public Task<UserEntity?> GetByIdAsync(string id);
        public Task UpdateAsync(string id, string name);
        public Task<string?> GetRoleByIdAsync(string id);
        public Task<IEnumerable<UserEntity>> GetManyByIdAsync(IEnumerable<string> ids);
        public Task DeleteByIdAsync(string id);
    }
}
