using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class RecipeProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Recipe, RecipeDto>()
                //.ForMember(dto => dto.MashSteps, conf => conf.MapFrom(rec => rec.MashSteps))
                //.ForMember(dto => dto.BoilSteps, conf => conf.MapFrom(rec => rec.BoilSteps))
                //.ForMember(dto => dto.FermentationSteps, conf => conf.MapFrom(rec => rec.FermentationSteps))
                .ForMember(dto => dto.Steps, conf => conf.ResolveUsing<StepsResolver>())
                .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));

            CreateMap<Recipe, RecipeSimpleDto>()
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                 .ForMember(dto => dto.RecipeId, conf => conf.MapFrom(rec => rec.RecipeId))
                 .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle));

            CreateMap<BeerStyle, DTO>()
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerStyleId))
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<RecipeDto,Recipe>()
                .ForMember(dto => dto.Notes, conf => conf.MapFrom(rec => rec.Notes))
                .ForMember(dto => dto.MashSteps, conf => conf.ResolveUsing<RecipeMashStepResolver>())
                .ForMember(dto => dto.BoilSteps, conf => conf.ResolveUsing<RecipeBoilStepResolver>())
                .ForMember(dto => dto.FermentationSteps, conf => conf.ResolveUsing<RecipeFermentationStepResolver>())
                .ForMember(dto => dto.Volume, conf => conf.MapFrom(rec => rec.Volume));
            
        }
    }
}