using Core.Entities;

namespace Application.Interfaces.Repositories
{
    public interface ITasksRepository
    {
        public Task CreateAsync(string name, string description, int score, DateTime start, DateTime end, string receiverId);
        public Task<IEnumerable<TaskEntity>> GetAllAsync();
        public Task<IEnumerable<TaskEntity>> GetByIdsAsync(IEnumerable<string> ids);
        public Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string id);
        public Task CompleteAsync(string id, string userId);
        public Task TakeAsync(string id, string userId);
    }
}