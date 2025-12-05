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
            var user = await _db.Users.Include(u => u.InProcessTasks).FirstOrDefaultAsync(u => u.Id == receiverId);

            if (user is null) return;

            TaskEntity task = new()
            {
                Name = name,
                Description = description,
                Score = score,
                StartDate = start,
                EndDate = end,
                Receiver = user,
                ReceiverId = receiverId,
                IsCompleted = false
            };

            user.InProcessTasks = [..user.InProcessTasks, task];

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
                .Include(t => t.Receiver)
                .ToArrayAsync();
        }

        /// <summary>
        /// Пометить задачу как выполненную
        /// </summary>
        public async Task CompleteAsync(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

            if (task is null)
                return;

            var user = await _db.Users.FirstOrDefaultAsync(t => t.Id == task.ReceiverId);

            if (user is null)
                return;

            task.IsCompleted = true;
            task.ActualEndDate = DateTime.UtcNow;

            user.Score += task.Score;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string id)
        {
            var inProcessTasks = await _db
                .Users.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => u.InProcessTasks)
                .FirstOrDefaultAsync() ?? [];

            var completedTasks = await _db
                .Users.AsNoTracking()
                .Where(u => u.Id == id)
                .Select(u => u.CompletedTasks)
                .FirstOrDefaultAsync() ?? [];

            return [.. completedTasks!, .. inProcessTasks!];
        }
    }
}
