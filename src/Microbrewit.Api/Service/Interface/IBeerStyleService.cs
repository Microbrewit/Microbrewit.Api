using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IBeerStyleService
    {
        Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size);
        Task<BeerStyleDto> GetSingleAsync(int id);
        Task<BeerStyleDto> AddAsync(BeerStyleDto beerStyleDto);
        Task<BeerStyleDto> DeleteAsync(int id);
        Task UpdateAsync(BeerStyleDto beerStyleDto);
        Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
