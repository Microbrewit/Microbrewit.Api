using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class HopProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Hop, HopDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Purpose, conf => conf.MapFrom(rec => rec.Purpose))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin))
                .ForMember(dto => dto.Oils, conf => conf.ResolveUsing<HopOilResolver>())
                .ForMember(dto => dto.Acids, conf => conf.ResolveUsing<HopAcidResolver>())
                .ForMember(dto => dto.AromaWheels, conf => conf.MapFrom(src => src.AromaWheels))
                .ForMember(dto => dto.Aliases, conf => conf.ResolveUsing<HopAliasesResolver>())
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopFlavoursResolver>())
                .ForMember(dto => dto.BeerStyles, conf => conf.ResolveUsing<HopBeerStylesResolver>())
                .ForMember(dto => dto.Substituts, conf => conf.MapFrom(rec => rec.Substituts));


            //CreateMap<HopFlavour, DTO>()
            //      .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Flavour.Name))
            //    .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FlavourId));

            CreateMap<Hop, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.HopId));

            CreateMap<Substitute, DTO>()
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Sub.Name))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SubstituteId));

            CreateMap<Origin, DTO>()
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OriginId));

            CreateMap<HopDto, Hop>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.AALow, conf => conf.MapFrom(rec => rec.Acids.AlphaAcid.Low))
                .ForMember(dto => dto.AAHigh, conf => conf.MapFrom(rec => rec.Acids.AlphaAcid.High))
                .ForMember(dto => dto.BetaLow, conf => conf.MapFrom(rec => rec.Acids.BetaAcid.Low))
                .ForMember(dto => dto.BetaHigh, conf => conf.MapFrom(rec => rec.Acids.BetaAcid.High))
                .ForMember(dto => dto.BPineneHigh, conf => conf.MapFrom(rec => rec.Oils.BPineneDto.High))
                .ForMember(dto => dto.BPineneLow, conf => conf.MapFrom(rec => rec.Oils.BPineneDto.Low))
                .ForMember(dto => dto.CaryophylleneHigh, conf => conf.MapFrom(rec => rec.Oils.CaryophylleneDto.High))
                .ForMember(dto => dto.CaryophylleneLow, conf => conf.MapFrom(rec => rec.Oils.CaryophylleneDto.Low))
                .ForMember(dto => dto.HumuleneHigh, conf => conf.MapFrom(rec => rec.Oils.HumuleneDto.High))
                .ForMember(dto => dto.HumuleneLow, conf => conf.MapFrom(rec => rec.Oils.HumuleneDto.Low))
                .ForMember(dto => dto.FarneseneHigh, conf => conf.MapFrom(rec => rec.Oils.FarneseneDto.High))
                .ForMember(dto => dto.FarneseneLow, conf => conf.MapFrom(rec => rec.Oils.FarneseneDto.Low))
                .ForMember(dto => dto.GeraniolHigh, conf => conf.MapFrom(rec => rec.Oils.GeraniolDto.High))
                .ForMember(dto => dto.GeraniolLow, conf => conf.MapFrom(rec => rec.Oils.GeraniolDto.Low))
                .ForMember(dto => dto.LinaloolHigh, conf => conf.MapFrom(rec => rec.Oils.LinalLoolDto.High))
                .ForMember(dto => dto.LinaloolLow, conf => conf.MapFrom(rec => rec.Oils.LinalLoolDto.Low))
                .ForMember(dto => dto.MyrceneHigh, conf => conf.MapFrom(rec => rec.Oils.MyrceneDto.High))
                .ForMember(dto => dto.MyrceneLow, conf => conf.MapFrom(rec => rec.Oils.MyrceneDto.Low))
                .ForMember(dto => dto.TotalOilHigh, conf => conf.MapFrom(rec => rec.Oils.TotalOilDto.High))
                .ForMember(dto => dto.TotalOilLow, conf => conf.MapFrom(rec => rec.Oils.TotalOilDto.Low))
                .ForMember(dto => dto.OtherOilHigh, conf => conf.MapFrom(rec => rec.Oils.OtherOilDto.High))
                .ForMember(dto => dto.OtherOilLow, conf => conf.MapFrom(rec => rec.Oils.OtherOilDto.Low))
                .ForMember(dto => dto.Aliases, conf => conf.ResolveUsing<HopPostAliasesResolver>())
                .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Origin.Id))
                .ForMember(dto => dto.Flavours, conf => conf.ResolveUsing<HopPostFlavoursResolver>())
                .ForMember(dto => dto.AromaWheels, conf => conf.MapFrom(rec => rec.AromaWheels))
                .ForMember(dto => dto.Substituts, conf => conf.ResolveUsing<SubstitutResolver>())
                .ForMember(dto => dto.HopBeerStyles, conf => conf.ResolveUsing<HopBeerStylesPostResolver>());

            //TODO: Check if AA is handles correct her.
            CreateMap<HopDto, HopStepDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.AAValue,
                    conf => conf.MapFrom(rec => (rec.Acids.AlphaAcid.Low + rec.Acids.AlphaAcid.High)/2))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
                
            CreateMap<DTO, Origin>()
                   .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                   .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Id));

            CreateMap<DTO, HopForm>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<HopForm, DTO>()
                  .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<AromaWheel,AromaWheelDto>()
            .ForMember(dto => dto.Id, conf => conf.MapFrom(source => source.Id))
            .ForMember(dto => dto.Name, conf => conf.MapFrom(source => source.Name));
            
            CreateMap<AromaWheelDto,AromaWheel>()
            .ForMember(db => db.Id, conf => conf.MapFrom(dto => dto.Id))
            .ForMember(db => db.Name, conf => conf.MapFrom(dto => dto.Name));
            
            //CreateMap<DTO,HopFlavour>()
            //     .ForMember(dto => dto.FlavourId, conf => conf.MapFrom(rec => rec.Id));
        }
    }
}