using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BoilStepHopResolver : ValueResolver<BoilStepDto,IList<BoilStepHop>>
    {
        protected override IList<BoilStepHop> ResolveCore(BoilStepDto boilStep)
        {
            var boilStepHops = new List<BoilStepHop>();
            foreach (var temp in boilStep.Ingredients.Where(i => i.Type == "hop"))
            {
                var hopStepDto = (HopStepDto) temp;
                var boilStepHop = AutoMapper.Mapper.Map<HopStepDto, BoilStepHop>(hopStepDto);
                boilStepHops.Add(boilStepHop);
            }
            return boilStepHops;
        }
    }
}
