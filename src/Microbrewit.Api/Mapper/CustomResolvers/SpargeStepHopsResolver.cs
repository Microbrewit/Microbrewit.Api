using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class SpargeStepHopsResolver : ValueResolver<SpargeStepDto,IList<SpargeStepHop>>
    {
        protected override IList<SpargeStepHop> ResolveCore(SpargeStepDto spargeStepDto)
        {
            var spargeStepHops = new List<SpargeStepHop>();
            foreach (var temp in spargeStepDto.Ingredients.Where(i => i.Type == "hop"))
            {
                var hopStepDto = (HopStepDto) temp;
                var spargeStepHop = AutoMapper.Mapper.Map<HopStepDto, SpargeStepHop>(hopStepDto);
                spargeStepHops.Add(spargeStepHop);
            }
            return spargeStepHops;
        }
    }
}
