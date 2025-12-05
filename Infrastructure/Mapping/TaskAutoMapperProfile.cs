using AutoMapper;
using Core.Entities;
using Core.Models.Tasks;

namespace Infrastructure.Mapping
{
    public class TaskAutoMapperProfile : Profile
    {
        public TaskAutoMapperProfile()
        {
            CreateMap<TaskEntity, TaskResponse>();
        }
    }
}