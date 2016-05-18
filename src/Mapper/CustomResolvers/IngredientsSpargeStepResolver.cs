using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class IngredientsSpargeStepResolver : ValueResolver<SpargeStep, IList<IIngredientStepDto>>
    {
        protected override IList<IIngredientStepDto> ResolveCore(SpargeStep source)
        {
            var ingredients = new List<IIngredientStepDto>();
            // var hops = Mapper.Map<ICollection<SpargeStepHop>, IList<HopStepDto>>(source.Hops);
            // ingredients.AddRange(hops);
            return ingredients;
        }
    }
}