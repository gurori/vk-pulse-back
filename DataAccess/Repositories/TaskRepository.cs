using Application.Interfaces.Repositories;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
    public sealed class TasksRepository(AppDbContext db) : ITasksRepository
    {
        private readonly AppDbContext _db = db;

        /// <summary>
        /// Создание новой задачи
        /// </summary>
        public async Task CreateAsync(string name, string description, int score, DateTime start, DateTime end, string receiverId)
        {
            var user = await _db.Users.Include(u => u.CreatedTasks).FirstOrDefaultAsync(u => u.Id == receiverId);

            if (user is null) return;

            TaskEntity task = new()
            {
                Name = name,
                Description = description,
                Score = score,
                StartDate = DateTime.SpecifyKind(start, DateTimeKind.Utc),
                EndDate = DateTime.SpecifyKind(end, DateTimeKind.Utc),
                Creator = user,
                CreatorId = receiverId,
                IsCompleted = false
            };

            user.CreatedTasks = [..user.CreatedTasks, task];

            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Получение задач по списку идентификаторов
        /// </summary>
        public async Task<IEnumerable<TaskEntity>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _db
                .Tasks.Where(t => ids.Contains(t.Id))
                .Include(t => t.Creator)
                .ToArrayAsync();
        }

        /// <summary>
        /// Пометить задачу как выполненную
        /// </summary>
        public async Task CompleteAsync(string id, string userId)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task is null || task.IsCompleted)
                return;

            var user = await _db.Users.FirstOrDefaultAsync(t => t.Id == userId);

            if (user is null)
                return;

            task.IsCompleted = true;
            task.ActualEndDate = DateTime.UtcNow;

            user.Score += task.Score;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string id)
        {
            return await _db.Tasks
                .AsNoTracking()
                .Include(t => t.Creator)
                .Where(t => t.CreatorId == id)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _db.Tasks
                .AsNoTracking()
                .Include(x => x.Creator)
                .Include(x => x.Receiver)
                .ToArrayAsync();
        }

        public async Task TakeAsync(string id, string userId)
        {
            var task = await _db.Tasks
                .Include(t => t.Receiver)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task is null || task.Receiver is not null)
                return;

            var user = await _db.Users
                .Include(u => u.ReceivedTasks)
                .Include(u => u.CreatedTasks)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null || user.ReceivedTasks.Select(t => t.Id).Contains(id))
                return;

            task.Receiver = user;
            task.ActualStartDate = DateTime.UtcNow;
            user.ReceivedTasks.Add(task);

            await _db.SaveChangesAsync();
        }
    }
}
