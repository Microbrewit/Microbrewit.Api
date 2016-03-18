using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IBeerStyleElasticsearch
    {
        Task UpdateAsync(BeerStyleDto beerStyleDto);
        Task<IEnumerable<BeerStyleDto>> GetAllAsync(int from, int size);
        Task<BeerStyleDto> GetSingleAsync(int id);
        Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<BeerStyleDto> beerStyleDtos);
        Task DeleteAsync(int id);
        BeerStyleDto GetSingle(int id);
        IEnumerable<BeerStyleDto> Search(string query, int from, int size);
    }
}
