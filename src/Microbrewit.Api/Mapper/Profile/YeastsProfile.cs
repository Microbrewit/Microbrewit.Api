using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class YeastsProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Yeast, YeastDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.YeastId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.Temperature, conf => conf.MapFrom(rec => new Temperature {High = rec.TemperatureHigh, Low = rec.TemperatureLow}))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.Flocculation, conf => conf.MapFrom(rec => new Flocculation {Low = rec.FlocculationLow, High = rec.FlocculationHigh, Label = rec.Flocculation}))
                .ForMember(dto => dto.AlcoholTolerance, conf => conf.MapFrom(rec => new AlcoholTolerance {Low = rec.AlcoholToleranceLow, High = rec.AlcoholToleranceHigh, Label = rec.AlcoholTolerance}))
                .ForMember(dto => dto.BrewerySource, conf => conf.MapFrom(rec => rec.BrewerySource))
                .ForMember(dto => dto.Species, conf => conf.MapFrom(rec => rec.Species))
                .ForMember(dto => dto.Attenuation, conf => conf.MapFrom(rec => new Attenuation{Low = rec.AttenuationLow, High = rec.AttenuationHigh, Label = rec.AttenuationRange}))
                .ForMember(dto => dto.Sources, conf => conf.MapFrom(rec => rec.Sources))
                .ForMember(dto => dto.PitchingFermentationNotes, conf => conf.MapFrom(rec => rec.PitchingFermentationNotes))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
               

            CreateMap<Supplier,DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SupplierId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<YeastDto,Yeast>()
                .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.TemperatureLow, conf => conf.MapFrom(rec => rec.Temperature.Low))
                .ForMember(dto => dto.TemperatureHigh, conf => conf.MapFrom(rec => rec.Temperature.High))
                .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.SubType))
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.Flocculation, conf => conf.MapFrom(rec => rec.Flocculation.Label))
                .ForMember(dto => dto.FlocculationLow, conf => conf.MapFrom(rec => rec.Flocculation.Low))
                .ForMember(dto => dto.FlocculationHigh, conf => conf.MapFrom(rec => rec.Flocculation.High))
                .ForMember(dto => dto.AlcoholTolerance, conf => conf.MapFrom(rec => rec.AlcoholTolerance.Label))
                .ForMember(dto => dto.AlcoholToleranceLow, conf => conf.MapFrom(rec => rec.AlcoholTolerance.Low))
                .ForMember(dto => dto.AlcoholToleranceHigh, conf => conf.MapFrom(rec => rec.AlcoholTolerance.High))
                .ForMember(dto => dto.AttenuationRange, conf => conf.MapFrom(rec => rec.Attenuation.Label))
                .ForMember(dto => dto.AttenuationLow, conf => conf.MapFrom(rec => rec.Attenuation.Low))
                .ForMember(dto => dto.AttenuationHigh, conf => conf.MapFrom(rec => rec.Attenuation.High))
                .ForMember(dto => dto.Sources, conf => conf.MapFrom(rec => rec.Sources))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier))
                .ForMember(dto => dto.SupplierId, conf => conf.ResolveUsing<YeastSupplierResolver>());

            CreateMap<YeastDto, YeastStepDto>()
                .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ProductCode, conf => conf.MapFrom(rec => rec.ProductCode))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.SubType))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));

            CreateMap<Source, SourceDto>()
             .ForMember(dto => dto.Site, conf => conf.MapFrom(rec => rec.Site))
             .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SocialId))
             .ForMember(dto => dto.Url, conf => conf.MapFrom(rec => rec.Url));

              CreateMap<SourceDto, Source>()
             .ForMember(dto => dto.Site, conf => conf.MapFrom(rec => rec.Site))
             .ForMember(dto => dto.SocialId, conf => conf.MapFrom(rec => rec.Id))
             .ForMember(dto => dto.Url, conf => conf.MapFrom(rec => rec.Url));
        }
    }
}