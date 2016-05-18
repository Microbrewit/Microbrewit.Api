using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopPostOriginResolver : ValueResolver<HopDto, int>
    {
       // private IOriginElasticsearch _originElasticsearch = new OriginElasticsearch();
      //  private IOriginRespository _originRespository = new OriginDapperRepository();
        protected override int ResolveCore(HopDto dto)
        {
                // if (dto.Origin != null)
                // {
                //     var originDto = _originElasticsearch.GetSingleAsync(dto.Origin.Id).Result;
                //     if (originDto != null)
                //         return originDto.Id;
                //     var origin = _originRespository.GetSingleAsync(dto.Origin.Id).Result;
                //     if (origin != null)
                //         return origin.OriginId;
                // }
            return 0;

        }
    }
}