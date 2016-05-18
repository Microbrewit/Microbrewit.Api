using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BeerStyleLinksResolver : ValueResolver<BeerStyle, BeerStyleLinks>
    {
        protected override BeerStyleLinks ResolveCore(BeerStyle beerStyle)
        {
            var beerStyleLinks = new BeerStyleLinks() { SubBeerStyleIds = new List<int>()};
            if (beerStyle.SubStyles != null)
            {
                foreach (var item in beerStyle.SubStyles)
                {
                    beerStyleLinks.SubBeerStyleIds.Add(item.BeerStyleId);
                }
            }
            return beerStyleLinks;
        }

    }
}