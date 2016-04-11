using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentationStepYeastsResolver : ValueResolver<FermentationStepDto,IList<FermentationStepYeast>>
    {
        protected override IList<FermentationStepYeast> ResolveCore(FermentationStepDto source)
        {
            var fermentationStepYeasts = new List<FermentationStepYeast>();
            // foreach (var temp in source.Ingredients.Where(i => i.Type == "yeast"))
            // {
            //     var yeastStepDto = (YeastStepDto) temp;
            //     var fermentationStepYeast = Mapper.Map<YeastStepDto, FermentationStepYeast>(yeastStepDto);
            //     fermentationStepYeasts.Add(fermentationStepYeast);
            // }
            return fermentationStepYeasts;
        }
    }
}
