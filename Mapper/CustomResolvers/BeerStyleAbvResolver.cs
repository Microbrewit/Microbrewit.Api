using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BeerStyleAbvResolver : ValueResolver<BeerStyle,Abv>
    {
        protected override Abv ResolveCore(BeerStyle beerStyle)
        {
            return new Abv {Low = beerStyle.ABVLow, High = beerStyle.ABVHigh};
        }
    }
}
