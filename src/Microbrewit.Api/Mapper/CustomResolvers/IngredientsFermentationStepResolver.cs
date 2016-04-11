using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class IngredientsFermentationStepResolver : ValueResolver<FermentationStep, IList<IIngredientStepDto>>
    {
        protected override IList<IIngredientStepDto> ResolveCore(FermentationStep source)
        {
            var ingredients = new List<IIngredientStepDto>();
            // var hops = Mapper.Map<ICollection<FermentationStepHop>, IList<HopStepDto>>(source.Hops);
            // ingredients.AddRange(hops);
            // var fermentables = Mapper.Map<ICollection<FermentationStepFermentable>, IList<FermentableStepDto>>(source.Fermentables);
            // ingredients.AddRange(fermentables);
            // var others = Mapper.Map<ICollection<FermentationStepOther>, IList<OtherStepDto>>(source.Others);
            // ingredients.AddRange(others);
            return ingredients;
        }
    }
}