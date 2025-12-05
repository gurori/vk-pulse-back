using Application.Interfaces.Repositories;
using AutoMapper;
using Core.Models.Tasks;

namespace Application.Services
{
    public sealed class TaskService(ITasksRepository tasksRepository, IMapper mapper)
    {
        private readonly ITasksRepository _tasksRepository = tasksRepository;
        private readonly IMapper _mapper = mapper;

        public async Task CreateAsync(TaskRequest request)
        {
            await _tasksRepository.CreateAsync(
                request.Name,
                request.Description,
                request.Score,
                request.StartDate,
                request.EndDate,
                request.ReceiverId
            );
        }

        public async Task<IEnumerable<TaskResponse>> Get(IEnumerable<string> ids)
        {
            var tasks = await _tasksRepository.GetByIdsAsync(ids);
            return _mapper.Map<TaskResponse[]>(tasks);
        }

        public async Task<IEnumerable<TaskResponse>> GetByUserId(string id)
        {
            var tasks = await _tasksRepository.GetByUserIdAsync(id);
            return _mapper.Map<TaskResponse[]>(tasks);
        }

        public async Task Complete(string id)
        {
            await _tasksRepository.CompleteAsync(id);
        }
    }
}