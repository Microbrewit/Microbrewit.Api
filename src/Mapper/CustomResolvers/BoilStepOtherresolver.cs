using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BoilStepOtherresolver : ValueResolver<BoilStepDto,IList<BoilStepOther>>
    {
        protected override IList<BoilStepOther> ResolveCore(BoilStepDto boilStepDto)
        {
            var boilStepOthers = new List<BoilStepOther>();
            foreach (var temp in boilStepDto.Ingredients.Where(i => i.Type == "other"))
            {
                var otherStepDto = (OtherStepDto) temp;
                //var boilStepOther = Mapper.Map<OtherStepDto,BoilStepOther>(otherStepDto);
                //boilStepOthers.Add(boilStepOther);
            }
            return boilStepOthers;
        }
    }
}
