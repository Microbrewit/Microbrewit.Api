using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class StepsResolver : ValueResolver<Recipe,IList<IStepDto>>
    {
        protected override IList<IStepDto> ResolveCore(Recipe source)
        {
            var steps = new List<IStepDto>();
            var mashSteps = AutoMapper.Mapper.Map<ICollection<MashStep>, IList<MashStepDto>>(source.MashSteps);
            steps.AddRange(mashSteps);
            var boilSteps = AutoMapper.Mapper.Map<ICollection<BoilStep>, IList<BoilStepDto>>(source.BoilSteps);
            steps.AddRange(boilSteps);
            var fermentationStep =
                AutoMapper.Mapper.Map<ICollection<FermentationStep>, IList<FermentationStepDto>>(source.FermentationSteps);
            steps.AddRange(fermentationStep);
            var spargeSteps = AutoMapper.Mapper.Map<IEnumerable<SpargeStep>, IList<SpargeStepDto>>(source.SpargeSteps);
            steps.AddRange(spargeSteps);           
            return steps;
        }
    }
}
