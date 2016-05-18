using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentationStepHopsResolver : ValueResolver<FermentationStepDto,IList<FermentationStepHop>>
    {
        protected override IList<FermentationStepHop> ResolveCore(FermentationStepDto source)
        {
            var fermentationStepHops = new List<FermentationStepHop>();
            // foreach (var temp in source.Ingredients.Where(i => i.Type == "hop"))
            // {
            //     var hopStepDto = (HopStepDto) temp;
            //     var fermentationStepHop = Mapper.Map<HopStepDto, FermentationStepHop>(hopStepDto);
            //     fermentationStepHops.Add(fermentationStepHop);
            // }
            return fermentationStepHops;
        }
    }
}
