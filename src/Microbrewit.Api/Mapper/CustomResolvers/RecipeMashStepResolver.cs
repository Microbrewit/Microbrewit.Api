using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class RecipeMashStepResolver : ValueResolver<RecipeDto,IList<MashStep>>
    {
        protected override IList<MashStep> ResolveCore(RecipeDto recipe)
        {
            var mashStepList = new List<MashStep>();
            foreach (var mashStepDto in recipe.Steps.OfType<MashStepDto>())
            {
                var mashStep = new MashStep()
                {
                    Fermentables = new List<MashStepFermentable>(),
                    Hops = new List<MashStepHop>(),
                    Others = new List<MashStepOther>(),
                  //  Id = mashStepDto.Id,
                    Length = mashStepDto.Length,
                    StepNumber = mashStepDto.StepNumber,
                    Notes = mashStepDto.Notes,
                    Volume = mashStepDto.Volume,
                    Temperature = mashStepDto.Temperature,
                };
                if (mashStepDto.Ingredients != null)
                {
                    foreach (var hopDto in mashStepDto.Ingredients.Where(i => i.Type == "hop"))
                    {
                        var temp = (HopStepDto) hopDto;
                        var hop = AutoMapper.Mapper.Map<HopStepDto, MashStepHop>(temp);
                        hop.StepNumber = mashStep.StepNumber;
                        mashStep.Hops.Add(hop);
                    }
                }
                if (mashStepDto.Ingredients != null)
                {

                    foreach (var fermentableDto in mashStepDto.Ingredients.Where(i => i.Type == "fermentable"))
                    {
                        var temp = (FermentableStepDto) fermentableDto;
                        var fermentable = AutoMapper.Mapper.Map<FermentableStepDto, MashStepFermentable>(temp);
                        if (fermentable == null) continue;
                        fermentable.StepNumber = mashStep.StepNumber;
                        mashStep.Fermentables.Add(fermentable);
                    }
                }

                if (mashStepDto.Ingredients != null)
                {

                    foreach (var otherDto in mashStepDto.Ingredients.Where(i => i.Type  == "other"))
                    {
                        var temp = (OtherStepDto) otherDto;
                        var other = AutoMapper.Mapper.Map<OtherStepDto, MashStepOther>(temp);
                        other.StepNumber = mashStep.StepNumber;
                        mashStep.Others.Add(other);

                    }
                }

                mashStepList.Add(mashStep);
            }

            return mashStepList;
        }
    }
}