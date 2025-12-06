// Application/Interfaces/Repositories/ITasksRepository.cs
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Application.Interfaces.Repositories
{
    public interface ITasksRepository
    {
        public Task<TaskEntity> CreateAsync(string name, string description, int score, DateTime start, DateTime end, string receiverId);
        public Task<IEnumerable<TaskEntity>> GetByIdsAsync(IEnumerable<string> ids);
        public Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string id);
        public Task CompleteAsync(string id);
        public Task UpdateAsync(TaskEntity task);
        public Task DeleteAsync(string taskId);
        public Task<TaskEntity?> GetByIdAndReceiverIdAsync(string taskId, string receiverId);
    }
}
