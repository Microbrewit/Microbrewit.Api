using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface IYeastElasticsearch
    {
        Task UpdateAsync(YeastDto yeastDto);
        Task<IEnumerable<YeastDto>> GetAllAsync();
        Task<YeastDto> GetSingleAsync(int id);
        Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<YeastDto> yeasts);
        Task DeleteAsync(int id);
        Task DeleteListAsync(IEnumerable<int> yeasts);
    }
}
