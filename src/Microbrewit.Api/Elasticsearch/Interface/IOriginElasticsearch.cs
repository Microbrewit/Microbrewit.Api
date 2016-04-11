using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IOriginElasticsearch
    {
        Task UpdateAsync(OriginDto originDto);

        Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size,string custom);
        Task<OriginDto> GetSingleAsync(int id);
        OriginDto GetSingle(int id);
        Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<OriginDto> originDtos);
        Task DeleteAsync(int id);
    }
}
