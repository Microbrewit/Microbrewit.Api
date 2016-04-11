using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class IngredientsBoilStepResolver : ValueResolver<BoilStep, IList<IIngredientStepDto>>
    {
        protected override IList<IIngredientStepDto> ResolveCore(BoilStep source)
        {
            var ingredients = new List<IIngredientStepDto>();
            // var hops = Mapper.Map<ICollection<BoilStepHop>, IList<HopStepDto>>(source.Hops);
            // ingredients.AddRange(hops);
            // var fermentables = Mapper.Map<ICollection<BoilStepFermentable>, IList<FermentableStepDto>>(source.Fermentables);
            // ingredients.AddRange(fermentables);
            // var others = Mapper.Map<ICollection<BoilStepOther>, IList<OtherStepDto>>(source.Others);
            // ingredients.AddRange(others);
            return ingredients;
        }
    }
}