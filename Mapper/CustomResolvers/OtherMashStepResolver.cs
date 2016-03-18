using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class OtherMashStepResolver : ValueResolver<MashStep, IList<OtherStepDto>>
    {
        // private readonly IOtherElasticsearch _otherElasticsearch = new OtherElasticsearch();
        // private readonly IOtherRepository _otherRepository = new OtherDapperRepository();

        protected override IList<OtherStepDto> ResolveCore(MashStep step)
        {
            var otherStepDtoList = new List<OtherStepDto>();
            // foreach (var item in step.Others)
            // {
            //     var otherStepDto = new OtherStepDto()
            //     {
            //         OtherId = item.OtherId,
            //         Amount = item.Amount,
            //     };
            //     var other = _otherElasticsearch.GetSingle(item.OtherId);
            //     if (other == null)
            //     {
            //         other = Mapper.Map<Other, OtherDto>(_otherRepository.GetSingle(item.OtherId));
            //     }
            //     otherStepDto.Name = other.Name;
            //     otherStepDto.SubType = other.Type;
            //     otherStepDtoList.Add(otherStepDto);
            // }
            return otherStepDtoList;

        }
    }
}