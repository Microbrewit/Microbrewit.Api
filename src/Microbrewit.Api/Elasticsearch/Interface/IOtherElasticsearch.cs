using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IOtherElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(OtherDto otherDto);
        Task<IEnumerable<OtherDto>> GetAllAsync(string custom);
        Task<OtherDto> GetSingleAsync(int id);
        Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<OtherDto> others);
        Task<IDeleteResponse> DeleteAsync(int id);
        OtherDto GetSingle(int id);
        IEnumerable<OtherDto> Search(string query, int from, int size);
        void Update(OtherDto otherDto);
    }
}
