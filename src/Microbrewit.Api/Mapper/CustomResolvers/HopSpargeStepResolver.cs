using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopSpargeStepResolver : ValueResolver<SpargeStep, IList<HopStepDto>>
    {
       // private readonly IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
       // private readonly IHopRepository _hopRepository = new HopDapperRepository();

        protected override IList<HopStepDto> ResolveCore(SpargeStep step)
        {
            var hopStepDtoList = new List<HopStepDto>();
            // foreach (var item in step.Hops)
            // {
            //     var hopStepDto = new HopStepDto()
            //     {
            //         HopId = item.HopId,
            //         Amount = item.AaAmount,
            //         AAValue = item.AaValue,
            //     };
            //     var hop = _hopElasticsearch.GetSingle(item.HopId) ?? Mapper.Map<Hop, HopDto>(_hopRepository.GetSingle(item.HopId));
            //     hopStepDto.Name = hop.Name;
            //     hopStepDto.Origin = hop.Origin;
            //     //hopStepDto.Flavours = hop.Flavours;
            //     //hopStepDto.FlavourDescription = hop.FlavourDescription;
            //     //TODO: Add elasticsearch on hop form.
            //     var hopForm = _hopRepository.GetForm(item.HopFormId) ?? _hopRepository.GetForm(1);
            //     hopStepDto.SubType = hopForm.Name;
            //     hopStepDtoList.Add(hopStepDto);
            // }
                return hopStepDtoList;
        }
    }
}