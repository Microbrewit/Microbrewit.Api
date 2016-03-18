using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class SpargeStepProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<SpargeStep, SpargeStepDto>()
                .ForMember(dest => dest.StepNumber, conf => conf.MapFrom(src => src.StepNumber))
                .ForMember(dto => dto.Type, conf => conf.UseValue("sparge"))
                .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Notes, conf => conf.MapFrom(src => src.Notes))
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
                .ForMember(dest => dest.Ingredients, conf => conf.ResolveUsing<IngredientsMashStepResolver>())
                .ForMember(dest => dest.Temperature, conf => conf.MapFrom(src => src.Temperature));
         
            CreateMap<SpargeStepDto, SpargeStep>()
               .ForMember(dest => dest.StepNumber, conf => conf.MapFrom(src => src.StepNumber))
               .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.Amount))
               .ForMember(dest => dest.Notes, conf => conf.MapFrom(src => src.Notes))
               .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.Type))
               .ForMember(dest => dest.Hops, conf => conf.ResolveUsing<SpargeStepHopsResolver>())
               .ForMember(dest => dest.Temperature, conf => conf.MapFrom(src => src.Temperature));

            CreateMap<SpargeStepHop, HopStepDto>()
                .ForMember(dest => dest.HopId, conf => conf.MapFrom(src => src.HopId))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Hop.Name))
                .ForMember(dest => dest.Origin, conf => conf.MapFrom(src => src.Hop.Origin.Name))
                .ForMember(dest => dest.Amount, conf => conf.MapFrom(src => src.AaAmount))
                .ForMember(dest => dest.AAValue, conf => conf.MapFrom(src => src.AaValue));
               //.ForMember(dest => dest.Flavours, conf => conf.MapFrom(src => src.Hop.Flavours))
               //.ForMember(dest => dest.FlavourDescription, conf => conf.MapFrom(src => src.Hop.FlavourDescription));

            CreateMap<HopStepDto, SpargeStepHop>()
              .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
              .ForMember(dto => dto.HopFormId, conf => conf.ResolveUsing<HopFromIdResolver>())
              .ForMember(dto => dto.AaAmount, conf => conf.MapFrom(rec => rec.Amount))
              .ForMember(dto => dto.AaValue, conf => conf.MapFrom(rec => rec.AAValue));
        }
    }
}