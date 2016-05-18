using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.Profile
{
    public class GlassProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Glass, GlassDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.GlassId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<GlassDto, Glass>()
               .ForMember(dto => dto.GlassId, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}