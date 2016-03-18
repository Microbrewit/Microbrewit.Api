using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopBeerStylesResolver : ValueResolver<Hop, IList<DTO>>
    {
        protected override IList<DTO> ResolveCore(Hop hop)
        {
            var beerStylesDto = new List<DTO>();
            foreach (var hopBeerStyle in hop.HopBeerStyles)
            {
                if(hopBeerStyle.BeerStyle != null)
                    beerStylesDto.Add(new DTO
                    {
                        Id = hopBeerStyle.BeerStyle.BeerStyleId,
                        Name = hopBeerStyle.BeerStyle.Name,
                        
                    });
            }

            return beerStylesDto;

        }
        }
}
