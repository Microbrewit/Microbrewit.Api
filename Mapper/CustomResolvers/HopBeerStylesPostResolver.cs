using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;


namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopBeerStylesPostResolver : ValueResolver<HopDto,IList<HopBeerStyle>>
    {
        protected override IList<HopBeerStyle> ResolveCore(HopDto hopDto)
        {
            var hopBeerStyles = new List<HopBeerStyle>();
            if (hopDto.BeerStyles == null) return hopBeerStyles;
            foreach (var dto in hopDto.BeerStyles)
            {
                hopBeerStyles.Add(new HopBeerStyle
                {
                    BeerStyleId = dto.Id,
                    HopId = hopDto.Id,
                });
            }

            return hopBeerStyles;

        }
    }
}
