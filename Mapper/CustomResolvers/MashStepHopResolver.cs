using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class MashStepHopResolver : ValueResolver<MashStepDto,IList<MashStepHop>>
    {
        protected override IList<MashStepHop> ResolveCore(MashStepDto mashStepDto)
        {
            var mashStepHops = new List<MashStepHop>();
//             foreach (var hopStepDto in mashStepDto.Ingredients.Where(i => i.Type == "hop"))
//             {
//                 var temp = (HopStepDto) hopStepDto;
//                 var hopStep = Mapper.Map<HopStepDto,MashStepHop>(temp);
//                 mashStepHops.Add(hopStep);
// 
//             }
            return mashStepHops;
        }
    }
}
