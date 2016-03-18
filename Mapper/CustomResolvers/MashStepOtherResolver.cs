using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class MashStepOtherResolver : ValueResolver<MashStepDto, IList<MashStepOther>>
    {
        protected override IList<MashStepOther> ResolveCore(MashStepDto mashStepDto)
        {
            var mashStepOthers = new List<MashStepOther>();
//             foreach (var ingredientStepDto in mashStepDto.Ingredients.Where(i => i.Type == "other"))
//             {
//                 var temp = (OtherStepDto)ingredientStepDto;
//                 var otherStep = Mapper.Map<OtherStepDto, MashStepOther>(temp);
//                 mashStepOthers.Add(otherStep);
// 
//             }
            return mashStepOthers;
        }
    }
}
