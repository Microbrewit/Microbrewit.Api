using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

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