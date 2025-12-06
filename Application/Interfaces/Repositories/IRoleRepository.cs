// Application/Interfaces/Repositories/IRoleRepository.cs
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<RoleEntity?> GetByIdAsync(int id);
        Task<IEnumerable<RoleEntity>> GetAllAsync();
        // Добавьте другие методы по мере необходимости
    }
}
