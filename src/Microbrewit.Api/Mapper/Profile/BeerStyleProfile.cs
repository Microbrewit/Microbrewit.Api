using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BeerStyleProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<BeerStyle, BeerStyleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerStyleId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Abv, conf => conf.MapFrom(rec => new Abv{ Low = rec.ABVLow, High = rec.ABVHigh }))
                .ForMember(dto => dto.Srm, conf => conf.MapFrom(rec => new Srm{Low = rec.SRMLow, High = rec.SRMHigh}))
                .ForMember(dto => dto.Ibu, conf => conf.MapFrom(rec => new Ibu{ Low = rec.IBULow, High = rec.IBUHigh }))
                .ForMember(dto => dto.Og, conf => conf.MapFrom(rec => new Og { Low = rec.OGLow, High = rec.OGHigh }))
                .ForMember(dto => dto.Fg, conf => conf.MapFrom(rec => new Fg { Low = rec.FGLow, High = rec.FGHigh }))
                .ForMember(dto => dto.SuperBeerStyle, conf => conf.MapFrom(rec => rec.SuperStyle))
                .ForMember(dto => dto.SubBeerStyles, conf => conf.MapFrom(rec => rec.SubStyles))
                .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<BeerStyleHopsResolver>())  ;


            CreateMap<BeerStyle, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerStyleId));

            CreateMap<BeerStyleSimpleDto, BeerStyle>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.BeerStyleId, conf => conf.MapFrom(rec => rec.Id));

            CreateMap<BeerStyle, BeerStyleSimpleDto>()
                  .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerStyleId));


            CreateMap<BeerStyleDto, BeerStyle>()
                .ForMember(dto => dto.BeerStyleId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABVHigh, conf => conf.MapFrom(rec => rec.Abv.High))
                .ForMember(dto => dto.ABVLow, conf => conf.MapFrom(rec => rec.Abv.Low))
                .ForMember(dto => dto.SRMHigh, conf => conf.MapFrom(rec => rec.Srm.High))
                .ForMember(dto => dto.SRMLow, conf => conf.MapFrom(rec => rec.Srm.Low))
                .ForMember(dto => dto.IBUHigh, conf => conf.MapFrom(rec => rec.Ibu.High))
                .ForMember(dto => dto.IBULow, conf => conf.MapFrom(rec => rec.Ibu.Low))
                .ForMember(dto => dto.OGHigh, conf => conf.MapFrom(rec => rec.Og.High))
                .ForMember(dto => dto.OGLow, conf => conf.MapFrom(rec => rec.Og.Low))
                .ForMember(dto => dto.FGHigh, conf => conf.MapFrom(rec => rec.Fg.High))
                .ForMember(dto => dto.FGLow, conf => conf.MapFrom(rec => rec.Fg.Low))
                .ForMember(dto => dto.SuperStyleId, conf => conf.MapFrom(rec => rec.SuperBeerStyle.Id));

        }
    }
}