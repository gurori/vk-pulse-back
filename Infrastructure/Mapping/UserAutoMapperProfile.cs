// Infrastructure/Mapping/UserAutoMapperProfile.cs
using AutoMapper;
using Core.Entities;
using Core.Models.Tasks; // Для TaskDto
using Core.Models.Users;
using System.Linq;

namespace Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<UserRegisterRequest, UserEntity>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Score, opt => opt.MapFrom(_ => 0))
                .ForMember(dest => dest.Team, opt => opt.Ignore())
                .ForMember(dest => dest.Position, opt => opt.Ignore())
                .ForMember(dest => dest.TasksReceived, opt => opt.Ignore());

            CreateMap<UserEntity, UserDto>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : null))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : null));

            CreateMap<UserEntity, UserResponse>()
                .ForMember(dest => dest.TeamName, opt => opt.MapFrom(src => src.Team != null ? src.Team.Name : null))
                .ForMember(dest => dest.PositionName, opt => opt.MapFrom(src => src.Position != null ? src.Position.Name : null))
                .ForMember(dest => dest.InProcessTasks, opt => opt.MapFrom(src => src.TasksReceived.Where(t => !t.IsCompleted)))
                .ForMember(dest => dest.CompletedTasks, opt => opt.MapFrom(src => src.TasksReceived.Where(t => t.IsCompleted)));
        }
    }
}
