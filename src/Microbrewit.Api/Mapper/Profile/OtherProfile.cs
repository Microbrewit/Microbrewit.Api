using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.Profile
{
    public class OtherProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Other, OtherDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OtherId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));

            CreateMap<OtherDto, Other>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));

            CreateMap<OtherDto, OtherStepDto>()
              .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.Id))
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
              .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.Type));
        }
    }
}