using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class MashStepFermentableResolver : ValueResolver<MashStepDto, IList<MashStepFermentable>>
    {
        protected override IList<MashStepFermentable> ResolveCore(MashStepDto mashStepDto)
        {
            var mashStepFermentagles = new List<MashStepFermentable>();
//             foreach (var fermentableStepDto in mashStepDto.Ingredients.Where(i => i.Type == "hop"))
//             {
//                 var temp = (FermentableStepDto) fermentableStepDto;
//                 var fermentalbeStep = Mapper.Map<FermentableStepDto, MashStepFermentable>(temp);
//                 mashStepFermentagles.Add(fermentalbeStep);
// 
//             }
            return mashStepFermentagles;
        }
    }
}
