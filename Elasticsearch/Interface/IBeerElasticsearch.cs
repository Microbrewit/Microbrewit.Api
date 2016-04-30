using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface IBeerElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(BeerDto beerDto);
        Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size);
        Task<BeerDto> GetSingleAsync(int id);
        Task<IEnumerable<BeerDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<BeerDto> beers);
        Task<IBulkResponse> ReIndexBulk(IEnumerable<BeerDto> beers, string index);
        Task<IDeleteResponse> DeleteAsync(int id);
        Task<IEnumerable<BeerDto>> GetLastAsync(int from, int size);
        Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username);
        Task<IEnumerable<BeerDto>> GetAllBreweryBeersAsync(int breweryId);
    }
}
