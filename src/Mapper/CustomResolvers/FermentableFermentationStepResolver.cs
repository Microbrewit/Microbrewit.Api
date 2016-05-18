using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class FermentableFermentationStepResolver : ValueResolver<FermentationStep, IList<FermentableStepDto>>
    {
        //private readonly IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        //private readonly IFermentableRepository _fermentableRepository = new FermentableDapperRepository();

        protected override IList<FermentableStepDto> ResolveCore(FermentationStep step)
        {
            var fermentableStepDtoList = new List<FermentableStepDto>();
            

                // foreach (var item in step.Fermentables)
                // {
                //     var fermentable = _fermentableElasticsearch.GetSingle(item.FermentableId);
                //     if (fermentable == null)
                //     {
                //         fermentable = Mapper.Map<Fermentable, FermentableDto>(_fermentableRepository.GetSingle(item.FermentableId));
                //     }
                //     var fermentableStepDto = new FermentableStepDto();
                //     
                //     fermentableStepDto.FermentableId = item.FermentableId;
                //     fermentableStepDto.Amount = item.Amount;
                //     fermentableStepDto.Supplier = fermentable.Supplier;
                //     fermentableStepDto.SubType = fermentable.Type;
                //     fermentableStepDto.Name = fermentable.Name;
                //     fermentableStepDto.PPG = fermentable.PPG;
                //         
                //     if(item.Lovibond == 0)
                //     {
                //         fermentableStepDto.Lovibond = item.Lovibond;
                //     }
                //     else
                //     {
                //         fermentableStepDto.Lovibond = fermentable.Lovibond;
                //     }
                //     fermentableStepDtoList.Add(fermentableStepDto);
                // }
            
            return fermentableStepDtoList;
        }

    }
}