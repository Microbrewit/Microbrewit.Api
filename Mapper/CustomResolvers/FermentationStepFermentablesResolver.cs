using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentationStepFermentablesResolver : ValueResolver<FermentationStepDto,IList<FermentationStepFermentable>>
    {
        protected override IList<FermentationStepFermentable> ResolveCore(FermentationStepDto source)
        {
            var fermentationStepFermentables = new List<FermentationStepFermentable>();
            // foreach (var temp in source.Ingredients.Where(i => i.Type == "fermentable"))
            // {
            //     var fermentableStepDto = (FermentableStepDto) temp;
            //     var fermentationStepHop = Mapper.Map<FermentableStepDto, FermentationStepFermentable>(fermentableStepDto);
            //     fermentationStepFermentables.Add(fermentationStepHop);
            // }
            return fermentationStepFermentables;
        }
    }
}
