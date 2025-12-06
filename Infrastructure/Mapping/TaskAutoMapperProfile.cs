// Infrastructure/Mapping/TaskAutoMapperProfile.cs
using AutoMapper;
using Core.Entities;
using Core.Models.Tasks;
using Core.Models.Users; // Для UserDto

namespace Infrastructure.Mapping
{
    public class TaskAutoMapperProfile : Profile
    {
        public TaskAutoMapperProfile()
        {
            CreateMap<TaskEntity, TaskDto>()
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId));

            CreateMap<TaskRequest, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(_ => false))
                .ForMember(dest => dest.ActualStartDate, opt => opt.Ignore())
                .ForMember(dest => dest.ActualEndDate, opt => opt.Ignore())
                .ForMember(dest => dest.Receiver, opt => opt.Ignore()); // Receiver будет установлен репозиторием

            CreateMap<TaskEntity, TaskResponse>()
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Receiver == null ? null : Mapper.Map<UserDto>(src.Receiver)));

            CreateMap<TaskDto, TaskEntity>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Receiver, opt => opt.Ignore())
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null)); // Обновлять только ненулевые поля
        }
    }
}
