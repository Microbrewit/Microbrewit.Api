using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BrewerProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<User, BrewerDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Username));
        }

    }
}