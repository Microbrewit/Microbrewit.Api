using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IYeastElasticsearch
    {
        Task UpdateAsync(YeastDto yeastDto);

        Task<IEnumerable<YeastDto>> GetAllAsync(string custom);
        Task<YeastDto> GetSingleAsync(int id);
        Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<YeastDto> yeasts);
        Task DeleteAsync(int id);
        YeastDto GetSingle(int id);
        IEnumerable<YeastDto> Search(string query, int from, int size);
    }
}
