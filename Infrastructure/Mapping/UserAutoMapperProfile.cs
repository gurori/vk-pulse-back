using AutoMapper;
using Core.Entities;
using Core.Models.Users;

namespace Infrastructure.Mapping
{
    public class UserAutoMapperProfile : Profile
    {
        public UserAutoMapperProfile()
        {
            CreateMap<UserEntity, UserResponse>();
        }
    }
}
