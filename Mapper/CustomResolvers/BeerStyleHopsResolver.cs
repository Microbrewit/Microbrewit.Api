using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BeerStyleHopsResolver : ValueResolver<BeerStyle,IList<DTO>>
    {
        protected override IList<DTO> ResolveCore(BeerStyle beerStyle)
        {
            var beerStylesDto = new List<DTO>();
            foreach (var hopBeerStyle in beerStyle.HopBeerStyles)
            {
                if(hopBeerStyle.Hop != null)
                    beerStylesDto.Add(new DTO
                    {
                        Id = hopBeerStyle.Hop.HopId,
                        Name = hopBeerStyle.Hop.Name,
                        
                    });
            }

            return beerStylesDto;

        }
        }
}
