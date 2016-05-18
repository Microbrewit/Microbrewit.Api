using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class IngredientsMashStepResolver : ValueResolver<MashStep, IList<IIngredientStepDto>>
    {
        protected override IList<IIngredientStepDto> ResolveCore(MashStep source)
        {
            var ingredients = new List<IIngredientStepDto>();
            // var hops = Mapper.Map<ICollection<MashStepHop>, IList<HopStepDto>>(source.Hops);
            // ingredients.AddRange(hops);
            // var fermentables = Mapper.Map<ICollection<MashStepFermentable>, IList<FermentableStepDto>>(source.Fermentables);
            // ingredients.AddRange(fermentables);
            // var others = Mapper.Map<ICollection<MashStepOther>, IList<OtherStepDto>>(source.Others);
            // ingredients.AddRange(others);
            return ingredients;
        }
    }
}