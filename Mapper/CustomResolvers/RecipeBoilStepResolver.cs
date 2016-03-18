using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class RecipeBoilStepResolver : ValueResolver<RecipeDto, IList<BoilStep>>
    {
        protected override IList<BoilStep> ResolveCore(RecipeDto recipe)
        {
            var boilStepList = new List<BoilStep>();
            {
                foreach (var item in recipe.Steps.Where(s => s.Type == "boil"))
                {
                    var boilStepDto = (BoilStepDto) item;
                    var boilStep = new BoilStep()
                    {
                        Fermentables = new List<BoilStepFermentable>(),
                        Hops = new List<BoilStepHop>(),
                        Others = new List<BoilStepOther>(),
                       // Id = boilStepDto.Id,
                        Length = boilStepDto.Length,
                        StepNumber = boilStepDto.StepNumber,
                        Notes = boilStepDto.Notes,
                        Volume = boilStepDto.Volume,
                    };
                    if (boilStepDto.Ingredients != null)
                    {
                        foreach (var temp in boilStepDto.Ingredients.Where(i => i.Type == "fermentable"))
                        {
                            var fermentableDto = (FermentableStepDto) temp;
                            var fermentable = AutoMapper.Mapper.Map<FermentableStepDto, BoilStepFermentable>(fermentableDto);
                            fermentable.StepNumber = boilStep.StepNumber;
                            boilStep.Fermentables.Add(fermentable);

                        }
                    }
                    if (boilStepDto.Ingredients != null)
                    {
                        foreach (var temp in boilStepDto.Ingredients.Where(i => i.Type == "hop"))
                        {
                            var hopDto =(HopStepDto) temp;
                            var hop = AutoMapper.Mapper.Map<HopStepDto, BoilStepHop>(hopDto);
                            hop.StepNumber = boilStepDto.StepNumber;
                            boilStep.Hops.Add(hop);
                        }
                    }

                    if (boilStepDto.Ingredients != null)
                    {
                        foreach (var temp in boilStepDto.Ingredients.Where(i => i.Type == "other"))
                        {
                            var otherDto = (OtherStepDto) temp;
                            var other = AutoMapper.Mapper.Map<OtherStepDto, BoilStepOther>(otherDto);
                            other.StepNumber = boilStepDto.StepNumber;
                            boilStep.Others.Add(other);
                        }
                    }

                    boilStepList.Add(boilStep);
                }

                return boilStepList;
            }
        }
    }
}