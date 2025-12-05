using AutoMapper;
using Core.Entities;
using Core.Models.Tasks;
using Core.Models.Teams;
using Core.Models.Users;

namespace Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<UserEntity, UserResponse>();
            CreateMap<UserEntity, UserDto>();

            CreateMap<TeamEntity, TeamDto>();

            CreateMap<TaskEntity, TaskDto>();
            CreateMap<TaskEntity, TaskResponse>();
        }
    }
}
