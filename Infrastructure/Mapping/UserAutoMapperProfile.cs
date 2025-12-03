using AutoMapper;
using Core.Entities;
using Core.Models.Response;

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
