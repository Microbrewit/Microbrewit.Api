using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BeerXmlBeerStyleSimpleResolver : ValueResolver<Recipe, BeerStyleSimpleDto>
    {
        //private IBeerStyleElasticsearch _beerStyleElasticsearch = new BeerStyleElasticsearch();

        protected override BeerStyleSimpleDto ResolveCore(Recipe recipe)
        {
            var beerStyleSimpleDto = new BeerStyleSimpleDto();
            //if (recipe.Style != null)
            //{
                // var beerStyle = _beerStyleElasticsearch.Search(recipe.Style.Name,0,1).FirstOrDefault();
                // if (beerStyle == null) return null;
                // beerStyleSimpleDto.Id = beerStyle.Id;
                // beerStyleSimpleDto.Name = beerStyle.Name;

                return beerStyleSimpleDto;
            //}
            //else
            //{
              //  return null;
            //}
            

        }
    }
}