// DataAccess/Repositories/TasksRepository.cs
using Application.Interfaces.Repositories;
using Core.Entities;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ulid;
using System;

namespace DataAccess.Repositories
{
    public sealed class TasksRepository(AppDbContext db) : ITasksRepository
    {
        private readonly AppDbContext _db = db;

        public async Task<TaskEntity> CreateAsync(string name, string description, int score, DateTime start, DateTime end, string receiverId)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == receiverId);
            if (user is null)
            {
                throw new InvalidOperationException($"Receiver user with ID {receiverId} not found.");
            }

            TaskEntity task = new()
            {
                Id = Ulid.NewUlid().ToString(),
                Name = name,
                Description = description,
                Score = score,
                StartDate = start,
                EndDate = end,
                ReceiverId = receiverId,
                IsCompleted = false,
                ActualStartDate = null,
                ActualEndDate = null,
                Receiver = user
            };

            await _db.Tasks.AddAsync(task);
            await _db.SaveChangesAsync();

            return task;
        }

        public async Task<IEnumerable<TaskEntity>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _db
                .Tasks.Where(t => ids.Contains(t.Id))
                .Include(t => t.Receiver)
                .ToArrayAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetByUserIdAsync(string id)
        {
            return await _db.Tasks
                .Include(t => t.Receiver)
                .Where(t => t.ReceiverId == id)
                .OrderBy(t => t.StartDate)
                .ToListAsync();
        }

        public async Task CompleteAsync(string id)
        {
            var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);
            if (task is null || task.IsCompleted) return;

            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == task.ReceiverId);
            if (user is null) return;

            task.IsCompleted = true;
            task.ActualEndDate = DateTime.UtcNow;

            user.Score += task.Score;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(TaskEntity task)
        {
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(string taskId)
        {
            var taskToDelete = await _db.Tasks.FindAsync(taskId);
            if (taskToDelete != null)
            {
                _db.Tasks.Remove(taskToDelete);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<TaskEntity?> GetByIdAndReceiverIdAsync(string taskId, string receiverId)
        {
            return await _db.Tasks
                .Include(t => t.Receiver)
                .FirstOrDefaultAsync(t => t.Id == taskId && t.ReceiverId == receiverId);
        }
    }
}
