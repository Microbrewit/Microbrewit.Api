using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class FermentationStepProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<FermentationStep, FermentationStepDto>()
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Ingredients, conf => conf.ResolveUsing<IngredientsFermentationStepResolver>());

            CreateMap<FermentationStepDto, FermentationStep>()
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Length, conf => conf.MapFrom(rec => rec.Length))
                .ForMember(dto => dto.Temperature, conf => conf.MapFrom(rec => rec.Temperature))
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes));

            CreateMap<FermentationStepHop, HopStepDto>()
                .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Hop.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Hop.Origin.Name))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));
                //.ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Hop.Flavours))
                //.ForMember(dto => dto.FlavourDescription, conf => conf.MapFrom(rec => rec.Hop.FlavourDescription));

            CreateMap<FermentationStepFermentable, FermentableStepDto>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Fermentable.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Fermentable.EBC))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.Fermentable.PPG))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Fermentable.Type))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<FermentationStepOther, OtherStepDto>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Other.Name))
               .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Other.Type))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<FermentationStepYeast, YeastStepDto>()
                 .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.YeastId))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Yeast.Name))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 //.ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
                 .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Yeast.Supplier));

            CreateMap<Supplier, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SupplierId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            // from web and to db
            CreateMap<FermentationStepDto, FermentationStep>()
                .ForMember(dto => dto.Hops, conf => conf.ResolveUsing<FermentationStepHopsResolver>())
                .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.StepNumber))
                .ForMember(dto => dto.Fermentables, conf => conf.ResolveUsing<FermentationStepFermentablesResolver>())
                .ForMember(dto => dto.Yeasts, conf => conf.ResolveUsing<FermentationStepYeastsResolver>())
                 .ForMember(dto => dto.Others, conf => conf.ResolveUsing<FermentationStepOthersResolver>());

            CreateMap<HopStepDto, FermentationStepHop>()
               .ForMember(dto => dto.HopId, conf => conf.MapFrom(rec => rec.HopId))
               .ForMember(dto => dto.HopFormId, conf => conf.ResolveUsing<HopFromIdResolver>())
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount))
               .ForMember(dto => dto.AAValue, conf => conf.MapFrom(rec => rec.AAValue));
            
            CreateMap<FermentableStepDto,FermentationStepFermentable>()
                .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<OtherStepDto, FermentationStepOther>()
               .ForMember(dto => dto.OtherId, conf => conf.MapFrom(rec => rec.OtherId))
               .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));

            CreateMap<YeastStepDto, FermentationStepYeast>()
                 .ForMember(dto => dto.YeastId, conf => conf.MapFrom(rec => rec.YeastId))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.StepNumber, conf => conf.MapFrom(rec => rec.Number))
                   .ForMember(dto => dto.Amount, conf => conf.MapFrom(rec => rec.Amount));
        }
    }
}