using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BeerStyleResolver : ValueResolver<Beer, BeerStyleSimpleDto>
    {
     //   private IBeerStyleElasticsearch _beerStyleElasticsearch = new BeerStyleElasticsearch();
     //   private IBeerStyleRepository _beerstyleRespository = new BeerStyleDapperRepository();

        protected override BeerStyleSimpleDto ResolveCore(Beer beer)
        {
            var beerStyleSimpleDto = new BeerStyleSimpleDto();
            if (beer.BeerStyleId != null)
            {
                // var beerStyle = _beerStyleElasticsearch.GetSingle((int)beer.BeerStyleId);
                // if (beerStyle == null)
                // {
                //     beerStyle = Mapper.Map<BeerStyle, BeerStyleDto>(_beerstyleRespository.GetSingle((int)beer.BeerStyleId));
                // }
                // beerStyleSimpleDto.Id = beerStyle.Id;
                // beerStyleSimpleDto.Name = beerStyle.Name;

                return beerStyleSimpleDto;
            } 
            else
	        {
                return null;
	        }
            

        }
    }
}