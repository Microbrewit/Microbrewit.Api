using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.Profile
{
    public class OriginProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Origin, OriginDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OriginId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<OriginDto, Origin>()
               .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}