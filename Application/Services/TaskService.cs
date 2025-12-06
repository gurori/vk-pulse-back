// Application/Services/TaskService.cs
using Core.Models.Tasks;
using Core.Entities;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITasksRepository _tasksRepository;
        private readonly IMapper _mapper;

        public TaskService(ITasksRepository tasksRepository, IMapper mapper)
        {
            _tasksRepository = tasksRepository;
            _mapper = mapper;
        }

        public async Task<TaskResponse> CreateAsync(TaskRequest request)
        {
            var createdTaskEntity = await _tasksRepository.CreateAsync(
                request.Name,
                request.Description,
                request.Score,
                request.StartDate,
                request.EndDate,
                request.ReceiverId
            );
            return _mapper.Map<TaskResponse>(createdTaskEntity);
        }

        public async Task<IEnumerable<TaskResponse>> GetByUserId(string userId)
        {
            var tasks = await _tasksRepository.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<TaskResponse>>(tasks);
        }

        public async Task Complete(string taskId, string currentUserId)
        {
            var task = await _tasksRepository.GetByIdAndReceiverIdAsync(taskId, currentUserId);
            if (task == null)
            {
                throw new UnauthorizedAccessException($"Task with ID {taskId} not found or does not belong to user {currentUserId}.");
            }
            if (task.IsCompleted)
            {
                throw new InvalidOperationException($"Task with ID {taskId} is already completed.");
            }
            await _tasksRepository.CompleteAsync(taskId);
        }

        public async Task<TaskResponse?> GetTaskByIdAsync(string taskId, string currentUserId)
        {
            var task = await _tasksRepository.GetByIdAndReceiverIdAsync(taskId, currentUserId);
            return _mapper.Map<TaskResponse>(task);
        }

        public async Task<TaskResponse?> UpdateTaskAsync(string taskId, TaskDto request, string currentUserId)
        {
            var existingTask = await _tasksRepository.GetByIdAndReceiverIdAsync(taskId, currentUserId);
            if (existingTask == null)
            {
                return null;
            }

            _mapper.Map(request, existingTask);
            // Логика для ActualStartDate/EndDate
            if (existingTask.IsCompleted && !existingTask.ActualEndDate.HasValue)
            {
                existingTask.ActualEndDate = DateTime.UtcNow;
            }
            else if (!existingTask.IsCompleted && existingTask.ActualEndDate.HasValue)
            {
                existingTask.ActualEndDate = null;
            }
            if (!existingTask.IsCompleted && existingTask.StartDate <= DateTime.UtcNow && !existingTask.ActualStartDate.HasValue)
            {
                existingTask.ActualStartDate = DateTime.UtcNow;
            }
            await _tasksRepository.UpdateAsync(existingTask);

            // Получаем обновленную задачу с Receiver, чтобы маппинг был полным
            var updatedTaskWithReceiver = await _tasksRepository.GetByIdAndReceiverIdAsync(existingTask.Id, existingTask.ReceiverId!);
            return _mapper.Map<TaskResponse>(updatedTaskWithReceiver);
        }

        public async Task<bool> DeleteTaskAsync(string taskId, string currentUserId)
        {
            var task = await _tasksRepository.GetByIdAndReceiverIdAsync(taskId, currentUserId);
            if (task == null)
            {
                return false;
            }
            await _tasksRepository.DeleteAsync(taskId);
            return true;
        }
    }
}
