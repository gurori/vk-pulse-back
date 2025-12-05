using AutoMapper;
using Core.Entities;
using Core.Models.Teams;

namespace Infrastructure.Mapping
{
    public class TeamAutoMapperProfile : Profile
    {
        public TeamAutoMapperProfile()
        {
            CreateMap<TeamEntity, TeamResponse>();
        }
    }
}
