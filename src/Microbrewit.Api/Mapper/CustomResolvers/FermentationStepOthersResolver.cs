using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentationStepOthersResolver : ValueResolver<FermentationStepDto,IList<FermentationStepOther>>
    {
        protected override IList<FermentationStepOther> ResolveCore(FermentationStepDto source)
        {
            var fermentationStepOthers = new List<FermentationStepOther>();
            // foreach (var temp in source.Ingredients.Where(i => i.Type == "other"))
            // {
            //     var otherStepDto = (OtherStepDto) temp;
            //     var fermentationStepOther = Mapper.Map<OtherStepDto, FermentationStepOther>(otherStepDto);
            //     fermentationStepOthers.Add(fermentationStepOther);
            // }
            return fermentationStepOthers;
        }
    }
}
