using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface IGlassElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(GlassDto glassDto);
        Task<IEnumerable<GlassDto>> GetAllAsync();
        Task<GlassDto> GetSingleAsync(int id);
        Task<IEnumerable<GlassDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<GlassDto> glasss);
        Task<IDeleteResponse> DeleteAsync(int id);
    }
}
