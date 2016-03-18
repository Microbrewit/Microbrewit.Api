using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IBeerService
    {
        Task<IEnumerable<BeerDto>> GetAllAsync(int from, int size);
        Task<BeerDto> GetSingleAsync(int id);
        Task<BeerDto> AddAsync(BeerDto beerDto);
        Task<BeerDto> AddAsync(BeerDto beerDto, string username);
        Task<BeerDto> DeleteAsync(int id);
        Task UpdateAsync(BeerDto beerDto);
        Task<IEnumerable<BeerDto>> SearchAsync(string query, int from, int size);
        Task<IEnumerable<BeerDto>> GetUserBeersAsync(string username); 
        Task ReIndexElasticSearch(string index);
        Task ReIndexSingleElasticSearchAsync(int beerId);
        Task<IEnumerable<BeerDto>> GetLastAsync(int @from, int size);
        Task<IEnumerable<BeerDto>> GetAllUserBeerAsync(string username);
        Task<IEnumerable<BeerDto>> GetAllBreweryBeersAsync(int breweryId);
    }
}
