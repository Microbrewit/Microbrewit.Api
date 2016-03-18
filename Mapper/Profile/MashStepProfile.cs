using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class MashStepProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<MashStep, MashStepDto>()
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Type, conf => conf.UseValue("mash"))
                .ForMember(dto => dto.Ingredients, conf => conf.ResolveUsing<IngredientsMashStepResolver>());

            CreateMap<MashStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.HopForm.Name))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));

            CreateMap<MashStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Fermentable.Supplier))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<MashStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<MashStepDto, MashStep>()
                .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<MashStepHopResolver>())
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Fermentables, conf => conf.ResolveUsing<MashStepFermentableResolver>())
                .ForMember(dto => dto.Others, conf => conf.ResolveUsing<MashStepOtherResolver>());

            CreateMap<HopStepDto, MashStepHop>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.HopFormId, conf => conf.ResolveUsing<HopFromIdResolver>())
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));

            CreateMap<FermentableStepDto, MashStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<OtherStepDto, MashStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));
        }
    }
}