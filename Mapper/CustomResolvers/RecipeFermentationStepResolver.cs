using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class RecipeFermentationStepResolver : ValueResolver<RecipeDto, IList<FermentationStep>>
    {
        protected override IList<FermentationStep> ResolveCore(RecipeDto recipe)
        {
            var fermentationStepList = new List<FermentationStep>();
            if(recipe == null || recipe.Steps == null) return fermentationStepList;
            foreach (var fermentationStepDto in recipe.Steps.OfType<FermentationStepDto>())
            {
                var fermentationStep = new FermentationStep()
                {
                    Fermentables = new List<FermentationStepFermentable>(),
                    Hops = new List<FermentationStepHop>(),
                    Others = new List<FermentationStepOther>(),
                    Yeasts = new List<FermentationStepYeast>(),
                  //  Id = fermentationStepDto.Id,
                    Length = fermentationStepDto.Length,
                    StepNumber = fermentationStepDto.StepNumber,
                    Notes = fermentationStepDto.Notes,
                    Temperature = fermentationStepDto.Temperature,
                };
                if (fermentationStepDto.Ingredients != null)
                {
                    foreach (var temp in fermentationStepDto.Ingredients.Where(i => i.Type == "hop"))
                    {
                        var hopDto = (HopStepDto) temp;
                        var hop = AutoMapper.Mapper.Map<HopStepDto, FermentationStepHop>(hopDto);
                      //  hop.StepNumber = fermentationStep.Id;
                        fermentationStep.Hops.Add(hop);
                    }
                }
                if (fermentationStepDto.Ingredients != null)
                {

                    foreach (var temp in fermentationStepDto.Ingredients.Where(i => i.Type == "fermentable"))
                    {
                        var fermentableDto = (FermentableStepDto) temp;
                        var fermentable = AutoMapper.Mapper.Map<FermentableStepDto, FermentationStepFermentable>(fermentableDto);
                        fermentable.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Fermentables.Add(fermentable);
                    }
                }

                if (fermentationStepDto.Ingredients != null)
                {

                    foreach (var temp in fermentationStepDto.Ingredients.Where(i => i.Type == "other"))
                    {
                        var otherDto = (OtherStepDto) temp;
                        var other = AutoMapper.Mapper.Map<OtherStepDto, FermentationStepOther>(otherDto);
                        other.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Others.Add(other);

                    }
                }

                if (fermentationStepDto.Ingredients != null)
                {

                    foreach (var temp in fermentationStepDto.Ingredients.Where(i => i.Type == "yeast"))
                    {
                        var yeastDto = (YeastStepDto) temp;
                        var yeast = AutoMapper.Mapper.Map<YeastStepDto, FermentationStepYeast>(yeastDto);
                        yeast.StepNumber = fermentationStep.StepNumber;
                        fermentationStep.Yeasts.Add(yeast);
                    }
                }
                fermentationStepList.Add(fermentationStep);
            }

            return fermentationStepList;
        }
    }
}