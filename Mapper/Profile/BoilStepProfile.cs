using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BoilStepProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<BoilStep, BoilStepDto>()
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Type, conf => conf.UseValue("boil"))
                .ForMember(dto => dto.Ingredients, conf => conf.ResolveUsing<IngredientsBoilStepResolver>());

            CreateMap<BoilStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.HopForm.Name))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));
                //.ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                //.ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            CreateMap<BoilStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<BoilStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<BoilStepDto, BoilStep>()
                 .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<BoilStepHopResolver>())
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                 .ForMember(dto => dto.Others, conf => conf.ResolveUsing<BoilStepOtherresolver>());

            CreateMap<HopStepDto, BoilStepHop>()
               .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
               .ForMember(dto => dto.HopFormId, conf => conf.ResolveUsing<HopFromIdResolver>())
               .ForMember(dto => dto.HopForm, conf => conf.ResolveUsing<SetHopFromNullResolver>())
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
               .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));

            CreateMap<FermentableStepDto, BoilStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<OtherStepDto, BoilStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));
        }
    }
}